using UnityEngine;

public class SimplePlayerShooting : MonoBehaviour
{
    private Camera mainCamera;
    private SimpleWeaponManager weaponManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        weaponManager = GetComponent<SimpleWeaponManager>();
    }

    private void Update()
    {
        if (mainCamera == null || weaponManager == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        weaponManager.AimAt(mousePosition);

        if (Input.GetMouseButton(0))
        {
            weaponManager.TryFire();
        }
    }
}
