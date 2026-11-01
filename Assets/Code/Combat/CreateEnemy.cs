using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public EnemyController enemyPrefab;
    private bool isCreated = false;
    public Transform spawnPoint;
    public float instantiateTime = 5f;

    private void Update()
    {
        if (isCreated == false)
        {
            StartCoroutine(CreatEnemys(instantiateTime));
            isCreated = true;
        }
          
    }

    private IEnumerator CreatEnemys(float sceonds)
    {
        
        yield return new WaitForSeconds(sceonds);
        Instantiate(enemyPrefab,spawnPoint.position, Quaternion.identity);
        isCreated = false;
        
    }
}
