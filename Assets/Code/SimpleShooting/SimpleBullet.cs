using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SimpleBullet : MonoBehaviour//在开火成功时被调用，用来处理子弹的移动和碰撞检测，挂载在子弹预制体上
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField, Min(1)] private int damage = 20;//造成伤害

    private Vector2 direction = Vector2.right;//子弹的移动方向
    private Transform owner;//玩家或敌人，避免子弹击中自己
    private CircleCollider2D hitbox;//获取子弹碰撞体，用于画检测圆
    private bool initialized;//已生成子弹判断
    private bool spent;//子弹已消失判断

    private void Awake()
    {
        hitbox = GetComponent<CircleCollider2D>();
        hitbox.isTrigger = true;
    }

    public void Initialize(Vector2 shootingDirection, Transform shooter = null)//子弹控制，由weaponController触发
    {
        direction = shootingDirection.normalized;
        owner = shooter;    
        initialized = true;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!initialized || spent) return;

        float distance = speed * Time.deltaTime;
        float radius = hitbox.radius * Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
        Vector2 origin = transform.TransformPoint(hitbox.offset);
        // 扫描整段飞行路径，避免速度快或目标较薄时从敌人身上穿过去。
        foreach (var hit in Physics2D.CircleCastAll(origin, radius, direction, distance))
        {
            if (TryHit(hit.collider)) return;
        }

        transform.position +=
            (Vector3)direction * distance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (initialized && !spent) TryHit(other);
    }

    private bool TryHit(Collider2D other)
    {
        if (other == hitbox || other.GetComponentInParent<SimpleBullet>() != null ||
            (owner != null && other.transform.IsChildOf(owner))) return false;//**判断other 所在的物体，是不是 owner 本身，或者 owner 的子物体。是的话就return

        var enemy = other.GetComponentInParent<EnemyController>();
        if (enemy != null)
        {
            var health = enemy.GetComponent<CharacterHealth>();
            if (health.IsDead) return false;
            // 命中无敌中的敌人也消耗子弹；同一颗子弹只结算一次。
            health.TakeDamage(damage, direction);
        }
        else if (other.isTrigger || other.GetComponentInParent<PlayerHealth>() != null)//如果碰到触发器或者检测到碰撞物体上有PlayerHealth这个组件，就继续下面的返回false
            return false;

        spent = true;
        Destroy(gameObject);
        return true;
    }
}
