using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
     public enum PotionType
    {
        HealthUp,
        SpeedUp,
        DamageUp,
        InvulnerableDurationUp,
        FireCoolDownUp
    }

    public PotionType potionType;




    public void UsePotion()
    {
        switch(potionType)
        {

        }
    }
}
