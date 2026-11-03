using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public PlayerStats playerStats;
    public CharacterHealth health;
     public enum PotionType
    {
        HealthUp,
        SpeedUp,
        DamageUp,
        InvulnerableDurationUp,
        FireCoolDownUp
    }

    public PotionType potionType;
    public float addAmount;

    private void Awake()
    {
        GameObject target = GameObject.Find("Player");
        health = target.GetComponent<CharacterHealth>();
        playerStats = target.GetComponent<PlayerStats>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UsePotion();
           
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }


    public void UsePotion()
    {
        switch(potionType)
        {
            case PotionType.HealthUp:
                HealthPo(addAmount);
                break;
            case PotionType.SpeedUp:
                SpeedPo(addAmount);
                break;
            case PotionType.DamageUp:
                DamagePo(addAmount);
                break;
            case PotionType.FireCoolDownUp:
                FireCoolDownPo(addAmount);
                break;
            case PotionType.InvulnerableDurationUp:
                InvulnerableDurationPo(addAmount);
                break;
            default:
                break;

        }
    }

    //触发各药瓶的具体逻辑单独写！！

    public void HealthPo(float amount)
    {
        playerStats.AddHealth(amount);
        health.RestoreHealth(amount);
    }


    public void SpeedPo(float amount)
    {
        playerStats.AddSpeed(amount);
    }


    public void DamagePo(float amount)
    {
        playerStats.AddDamage(amount);
    }

    public void FireCoolDownPo(float amount)
    {
        playerStats.AddFireBaseCooldown(amount);
    }

    public void InvulnerableDurationPo(float amount)
    {
        playerStats.AddInvulnerableDuration(amount);
    }    
}
