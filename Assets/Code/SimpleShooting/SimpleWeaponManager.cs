using UnityEngine;

public class SimpleWeaponManager : MonoBehaviour
{
    [SerializeField]
    private SimpleWeaponController currentWeapon;

    public void AimAt(Vector2 targetPosition)
    {
        if (currentWeapon == null)
            return;

        currentWeapon.AimAt(targetPosition);
    }

    public bool TryFire()
    {
        if (currentWeapon == null)
            return false;

        return currentWeapon.TryFire();
    }
}
