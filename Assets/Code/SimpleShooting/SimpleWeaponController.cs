using UnityEngine;

public class SimpleWeaponController : MonoBehaviour//挂载到武器上的武器控制脚本，通过中转站的玩家输入信息传到这里
{
    [Header("武器配置")]
    [SerializeField] private SimpleBullet bulletPrefab;//子弹预制体
    [SerializeField] private Transform muzzlePoint;//决定生成预制体的位置
    private PlayerStats playerStats;//玩家的统一数值组件
    private BulletNumberController bulletNumberControll;
    private float nextFireTime;//处理开火间隔的数值
    private Vector2 aimDirection = Vector2.right;//武器朝向

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();//武器挂在玩家的子物体上
        bulletNumberControll = GetComponent<BulletNumberController>();
    }

    public void AimAt(Vector2 targetPosition)//接收来自玩家的鼠标输入，转化成武器的移动
    {
        Vector2 direction =
            targetPosition - (Vector2)transform.position;//求direction的方向向量，目标位置 − 武器位置 = 武器指向目标的向量，很重要。

        if (direction.sqrMagnitude < 0.0001f)
            return;//如果方向向量的平方长度小于0.0001f，说明目标位置和武器位置几乎重合，直接返回。

        aimDirection = direction.normalized;//将方向向量归一化，得到单位向量，表示武器的朝向。

        float angle = Mathf.Atan2(
            aimDirection.y,
            aimDirection.x
        ) * Mathf.Rad2Deg;//使用Atan2函数计算武器的旋转角度，返回值是弧度，需要乘以Rad2Deg转换为角度。

        transform.rotation = Quaternion.Euler(0f, 0f, angle);//将武器的旋转设置为计算出的角度，使用Quaternion.Euler创建一个绕Z轴旋转的四元数。由于z轴垂直于2D平面，所以我们只需要设置z轴的旋转角度。
    }

    public bool TryFire()//返回bool值，告诉前面的脚本是否成功开火
    {
        if(bulletNumberControll.currentBullets < 1) return false;
        if (Time.time < nextFireTime)//如果当前时间小于下次开火时间，说明还在冷却中，无法开火，直接返回false。
            return false;

        if (bulletPrefab == null || muzzlePoint == null || playerStats == null)
            return false;

        SimpleBullet bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            Quaternion.identity
        );//生成子弹预制体，位置为muzzlePoint的位置，旋转为默认值Quaternion.identity。**把新子弹的组件存入刚声明的bullet里，方便后续调用

        bulletNumberControll.ConsumeBullets();
        // 生成子弹后，调用子弹的初始化方法并传入瞄准方向。
        bullet.Initialize(aimDirection, playerStats.transform, playerStats.playerTakeDamage);//传入方向、玩家和本次开火的伤害

        nextFireTime = Time.time + Mathf.Max(0f, playerStats.fireCooldown);//从 PlayerStats 读取最新开火间隔。
        return true;
    }
}
