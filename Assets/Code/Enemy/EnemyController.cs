using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterHealth))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Hurt, Dead }//巡逻，追击，受伤，死亡

    [Header("生命")]
    [SerializeField, Min(1)] private int maxHealth = 100;//敌人最大生命
    [SerializeField, Min(0f)] private float invulnerableDuration = 0.5f;//敌人受伤后的无敌时间

    public int MaxHealth => maxHealth;
    public float InvulnerableDuration => invulnerableDuration;

    [Header("巡逻")]
    [SerializeField, Min(0f)] private float patrolSpeed = 1.2f;//巡逻速度
    [SerializeField, Min(0f)] private float patrolRadius = 2f;//巡逻圆
    [SerializeField, Min(0f)] private float waitTime = 1f;//等待时间

    [Header("追击")]
    [SerializeField] private PlayerHealth target;
    [SerializeField, Min(0f)] private float detectionRadius = 6f;//检测玩家的圆半径
    [SerializeField, Min(0f)] private float chaseSpeed = 3f;
    [SerializeField, Min(0f)] private float lostTargetTime = 2f;
    [SerializeField, Min(0f)] private float stoppingDistance = 0.4f;//？

    [Header("战斗")]
    [SerializeField, Min(1)] private int contactDamage = 10;
    [SerializeField, Min(0.01f)] private float contactCooldown = 1f;
    [SerializeField, Min(0f)] private float hurtDuration = 0.3f;
    [SerializeField, Min(0f)] private float knockbackSpeed = 4f;
    [SerializeField, Min(0.1f)] private float deathDelay = 0.5f;

    public EnemyState CurrentState { get; private set; }

    // 状态类通过这些只读属性使用配置，Inspector 中的字段仍保留在控制器里。
    internal Rigidbody2D Body => body;
    internal Vector2 HomePosition => homePosition;
    internal Vector2 LastSeenPosition => lastSeenPosition;
    internal float PatrolSpeed => patrolSpeed;
    internal float PatrolRadius => patrolRadius;
    internal float WaitTime => waitTime;
    internal float ChaseSpeed => chaseSpeed;
    internal float StoppingDistance => stoppingDistance;
    internal float HurtDuration => hurtDuration;
    internal float KnockbackSpeed => knockbackSpeed;
    internal float DeathDelay => deathDelay;
    internal bool FoundPlayer { get; private set; }
    internal bool RemembersPlayer => target != null && target.IsAlive &&
        Time.time - lastSeenTime < lostTargetTime;

    private Rigidbody2D body;
    private CharacterHealth health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 homePosition;
    private Vector2 lastSeenPosition;
    private float lastSeenTime;
    private float nextContactTime;
    private float nextTargetSearchTime;

    private EnemyStateBase currentState;
    private EnemyPatrolState patrolState;
    private EnemyChaseState chaseState;
    private EnemyHurtState hurtState;
    private EnemyDeadState deadState;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<CharacterHealth>();
        health.InitializeEnemy(this);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body.gravityScale = 0f;
        body.freezeRotation = true;
        homePosition = body.position;

        // 每只敌人各自创建状态对象，不需要把状态脚本挂到 GameObject 上。
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        hurtState = new EnemyHurtState(this);
        deadState = new EnemyDeadState(this);
        SwitchState(EnemyState.Patrol);
    }

    private void OnEnable()
    {
        health.Damaged += OnHurt;
        health.Died += OnDead;
    }

    private void OnDisable()
    {
        health.Damaged -= OnHurt;
        health.Died -= OnDead;
        body.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (health.IsDead) return;

        if (target == null && Time.time >= nextTargetSearchTime)
        {
            target = FindObjectOfType<PlayerHealth>();
            nextTargetSearchTime = Time.time + 0.5f;
        }

        FoundPlayer = target != null && target.IsAlive &&
            Vector2.Distance(body.position, target.transform.position) <= detectionRadius;

        if (FoundPlayer)
        {
            lastSeenPosition = target.transform.position;
            lastSeenTime = Time.time;
        }

        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    // 公共移动能力：具体目标、速度和等待条件由当前状态决定。
    internal void MoveTowards(Vector2 destination, float speed, float stopDistance, bool waiting = false)
    {
        Vector2 offset = destination - body.position;

        // velocity 的单位是“单位/秒”，这里不乘 Time.deltaTime。
        body.velocity = waiting || offset.magnitude <= stopDistance ?
            Vector2.zero : offset.normalized * speed;

        if (spriteRenderer != null && Mathf.Abs(body.velocity.x) > 0.01f)
            spriteRenderer.flipX = body.velocity.x > 0f; // 原野猪贴图默认朝左。

        SetAnimationBool("walk", CurrentState == EnemyState.Patrol && body.velocity.sqrMagnitude > 0.01f);
        SetAnimationBool("run", CurrentState == EnemyState.Chase && body.velocity.sqrMagnitude > 0.01f);
    }

    internal void SetAnimationBool(string parameter, bool value)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool(parameter, value);
    }

    internal void TriggerAnimation(string parameter)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger(parameter);
    }

    internal void SwitchState(EnemyState state)
    {
        if (currentState != null && CurrentState == state) return;

        EnemyStateBase newState = state switch
        {
            EnemyState.Patrol => patrolState,
            EnemyState.Chase => chaseState,
            EnemyState.Hurt => hurtState,
            EnemyState.Dead => deadState,
            _ => throw new System.ArgumentOutOfRangeException(nameof(state), state, null)
        };

        currentState?.OnExit();
        currentState = newState;
        CurrentState = state;
        currentState.OnEnter();
    }

    private void OnHurt(Vector2 hitDirection)
    {
        SwitchState(EnemyState.Hurt);
        hurtState.ApplyHit(hitDirection);
    }

    private void OnDead()
    {
        SwitchState(EnemyState.Dead);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (health.IsDead || CurrentState == EnemyState.Hurt || Time.time < nextContactTime) return;
        var player = other.GetComponentInParent<PlayerHealth>();
        if (player == null || !player.IsAlive) return;

        Vector2 direction = other.transform.position - transform.position;
        if (player.GetComponent<CharacterHealth>().TakeDamage(contactDamage, direction))
            nextContactTime = Time.time + contactCooldown;
    }

    // 兼容原 dead.anim 中已经存在的同名事件（原项目拼写如此）。
    public void DestoryAfterAnim()
    {
        if (health.IsDead) Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)homePosition : transform.position, patrolRadius);
    }
}
