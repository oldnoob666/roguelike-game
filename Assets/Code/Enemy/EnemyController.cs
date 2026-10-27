using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterHealth))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Hurt, Dead }

    [Header("巡逻")]
    [SerializeField, Min(0f)] private float patrolSpeed = 1.2f;
    [SerializeField, Min(0f)] private float patrolRadius = 2f;
    [SerializeField, Min(0f)] private float waitTime = 1f;

    [Header("追击")]
    [SerializeField] private PlayerHealth target;
    [SerializeField, Min(0f)] private float detectionRadius = 6f;
    [SerializeField, Min(0f)] private float chaseSpeed = 3f;
    [SerializeField, Min(0f)] private float lostTargetTime = 2f;
    [SerializeField, Min(0f)] private float stoppingDistance = 0.4f;

    [Header("战斗")]
    [SerializeField, Min(1)] private int contactDamage = 10;
    [SerializeField, Min(0.01f)] private float contactCooldown = 1f;
    [SerializeField, Min(0f)] private float hurtDuration = 0.3f;
    [SerializeField, Min(0f)] private float knockbackSpeed = 4f;
    [SerializeField, Min(0.1f)] private float deathDelay = 0.5f;

    public EnemyState CurrentState { get; private set; }

    private Rigidbody2D body;
    private CharacterHealth health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 homePosition;
    private Vector2 patrolPoint;
    private Vector2 lastSeenPosition;
    private float waitUntil;
    private float lastSeenTime;
    private float hurtUntil;
    private float nextContactTime;
    private float nextTargetSearchTime;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<CharacterHealth>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body.gravityScale = 0f;
        body.freezeRotation = true;
        homePosition = body.position;
        patrolPoint = homePosition;
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

        bool foundPlayer = target != null && target.IsAlive &&
            Vector2.Distance(body.position, target.transform.position) <= detectionRadius;

        if (foundPlayer)
        {
            lastSeenPosition = target.transform.position;
            lastSeenTime = Time.time;
        }

        if (CurrentState == EnemyState.Hurt && Time.time < hurtUntil) return;

        if (foundPlayer || (CurrentState != EnemyState.Patrol && target != null &&
            target.IsAlive && Time.time - lastSeenTime < lostTargetTime))
            SwitchState(EnemyState.Chase);
        else
            SwitchState(EnemyState.Patrol);

        if (CurrentState == EnemyState.Patrol &&
            Vector2.Distance(body.position, patrolPoint) < 0.15f && Time.time >= waitUntil)
        {
            patrolPoint = homePosition + Random.insideUnitCircle * patrolRadius;
            waitUntil = Time.time + waitTime;
        }
    }

    private void FixedUpdate()
    {
        if (CurrentState == EnemyState.Dead || CurrentState == EnemyState.Hurt) return;

        Vector2 destination = CurrentState == EnemyState.Chase ? lastSeenPosition : patrolPoint;
        Vector2 offset = destination - body.position;
        float stopDistance = CurrentState == EnemyState.Chase ? stoppingDistance : 0.15f;
        bool waiting = CurrentState == EnemyState.Patrol && Time.time < waitUntil;
        float speed = CurrentState == EnemyState.Chase ? chaseSpeed : patrolSpeed;

        // velocity 的单位是“单位/秒”，这里不乘 Time.deltaTime。
        body.velocity = waiting || offset.magnitude <= stopDistance ?
            Vector2.zero : offset.normalized * speed;

        if (spriteRenderer != null && Mathf.Abs(body.velocity.x) > 0.01f)
            spriteRenderer.flipX = body.velocity.x > 0f; // 原野猪贴图默认朝左。

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("walk", CurrentState == EnemyState.Patrol && body.velocity.sqrMagnitude > 0.01f);
            animator.SetBool("run", CurrentState == EnemyState.Chase && body.velocity.sqrMagnitude > 0.01f);
        }
    }

    private void SwitchState(EnemyState state)
    {
        if (CurrentState == state) return;
        CurrentState = state;
        if (state == EnemyState.Patrol)
        {
            patrolPoint = homePosition;
            waitUntil = Time.time;
        }
    }

    private void OnHurt(Vector2 hitDirection)
    {
        SwitchState(EnemyState.Hurt);
        hurtUntil = Time.time + hurtDuration;
        body.velocity = hitDirection * knockbackSpeed;
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger("hurt");
    }

    private void OnDead()
    {
        SwitchState(EnemyState.Dead);
        body.velocity = Vector2.zero;
        foreach (var hitbox in GetComponentsInChildren<Collider2D>())
            hitbox.enabled = false;
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool("dead", true);
        // 即使没有动画，或者动画事件丢失，也保证能销毁尸体。
        Destroy(gameObject, deathDelay);
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
