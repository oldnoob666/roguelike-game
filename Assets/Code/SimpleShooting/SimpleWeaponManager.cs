using UnityEngine;

public class SimpleWeaponManager : MonoBehaviour//中转站作用，负责接收SimplePlayerShooting里玩家的鼠标输入，并转给SimpleWeaponController
{
    [SerializeField]
    private SimpleWeaponController currentWeapon;

    public void AimAt(Vector2 targetPosition)
    {
        if (currentWeapon == null)
            return;

        currentWeapon.AimAt(targetPosition);//把targetPosition传递到currentWeapon里的AimAt函数
    }

    public bool TryFire()
    {
        if (currentWeapon == null)
            return false;

        return currentWeapon.TryFire();//调用SimpleWeaponController里的TryFire
    }
}
