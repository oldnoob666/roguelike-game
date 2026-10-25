using UnityEngine;

public class SimpleWeaponController : MonoBehaviour
{
    [Header("武器配置")]
    [SerializeField] private SimpleBullet bulletPrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float fireCooldown = 0.2f;

    private float nextFireTime;
    private Vector2 aimDirection = Vector2.right;

    public void AimAt(Vector2 targetPosition)
    {
        Vector2 direction =
            targetPosition - (Vector2)transform.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        aimDirection = direction.normalized;

        float angle = Mathf.Atan2(
            aimDirection.y,
            aimDirection.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public bool TryFire()
    {
        if (Time.time < nextFireTime)
            return false;

        if (bulletPrefab == null || muzzlePoint == null)
            return false;

        SimpleBullet bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            Quaternion.identity
        );

        // 生成子弹后，调用子弹的初始化方法并传入瞄准方向。
        bullet.Initialize(aimDirection);

        nextFireTime = Time.time + fireCooldown;
        return true;
    }
}
