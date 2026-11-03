using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletNumberController : MonoBehaviour
{
    public PlayerStats playerStats;
    public float currentBullets;
    [SerializeField] private TMP_Text bulletText;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        currentBullets = playerStats.bulletsNumber;
    }


    public void ConsumeBullets()
    {
        currentBullets--;
        if(currentBullets < 0)
        {
            currentBullets = 0;
        }
    }


    public void RestoreBullet( float amount)
    {
        currentBullets += amount;
        if(currentBullets > playerStats.bulletsNumber)
        {
            currentBullets = playerStats.bulletsNumber;
        }
    }

    private void Update()
    {
        bulletText.text = $"Bullet :{currentBullets} /{playerStats.bulletsNumber}";
    }



}
