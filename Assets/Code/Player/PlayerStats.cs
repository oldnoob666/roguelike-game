using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playerBaseSpeed = 5f;
    public float playerBaseHealth = 100f;
    public float playerBaseTakeDamage = 20f;//玩家武器伤害
    public float playerBaseInvulnerableDuration = 0.5f;//无敌时间
    public float fireBaseCooldown = 0.2f;//冷却间隔


    private float speedBonus;
    private float playerHealthBonus;
    private float playerTakeDamageBonus;
    private float fireCooldownBonus;
    private float playerBaseinvulnerableDurationBonus;

    public float playerSpeed => playerBaseSpeed + speedBonus;//只读属性，每次调用计算一次，并且在unity面板上不显示
    public float playerHealth => playerBaseHealth + playerHealthBonus;
    public float playerTakeDamage => playerBaseTakeDamage + playerTakeDamageBonus;
    public float playerinvulnerableDuration => playerBaseInvulnerableDuration + playerBaseinvulnerableDurationBonus;
    public float fireCooldown => fireBaseCooldown + fireCooldownBonus;

    public void AddSpeed(float amount)
    {
        speedBonus += amount;
    }

    public void AddHealth(float amount)
    {
        playerHealthBonus += amount;
    }

    public void AddDamage(float amount)
    {
        playerTakeDamageBonus += amount;
    }

    public void AddFireBaseCooldown(float amount)
    {
        fireCooldownBonus += amount;
    }

    public void AddInvulnerableDuration(float amount)
    {
        playerBaseinvulnerableDurationBonus += amount;
    }

}
