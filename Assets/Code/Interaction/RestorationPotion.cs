using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationPotion : MonoBehaviour
{
    public enum PotionType
    {
        HealthRestore,

    }
    public PotionType potionType;
    public float addAmount;
    public CharacterHealth health;


    private void Awake()
    {
        GameObject target = GameObject.Find("Player");
        health = target.GetComponent<CharacterHealth>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UsePotion();
            Debug.Log("触发");
            Destroy(gameObject);
        }
    }



    public void UsePotion()
    {
        switch(potionType)
        {
            case PotionType.HealthRestore:
                ReHealth(addAmount);
                break;
        }
    }

    public void ReHealth(float amount)
    {
        health.RestoreHealth(amount);
    }
}
