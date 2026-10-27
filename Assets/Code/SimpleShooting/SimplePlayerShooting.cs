using UnityEngine;

public class SimplePlayerShooting : MonoBehaviour//这个脚本负责接收玩家的鼠标移动输入，点击输入
{
    private Camera mainCamera;
    private SimpleWeaponManager weaponManager;

    private void Awake()
    {
        mainCamera = Camera.main;//获取主摄像机，以便将鼠标位置转换为世界坐标（2D射击）
        weaponManager = GetComponent<SimpleWeaponManager>();//获取SimpleWeaponManager组件，以便通过该组件控制武器的瞄准和射击
    }

    private void Update()
    {
        if (mainCamera == null || weaponManager == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);//把鼠标的世界坐标赋值给mousePosition

        weaponManager.AimAt(mousePosition);//把mousePosition传递到weaponManager里的AimAt函数

        if (Input.GetMouseButton(0))
        {
            weaponManager.TryFire();//如果按鼠标左键，触发weaponManager的TryFire函数
        }
    }
}
