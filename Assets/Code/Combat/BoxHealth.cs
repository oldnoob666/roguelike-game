using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : MonoBehaviour
{
    public CharacterHealth health;
    


    private void Awake()
    {
        health = GetComponent<CharacterHealth>();
    }



    private void OnEnable()
    {
        health.Damaged += OnHurt;
        health.Died += OnDead;
    }


    private void OnDestroy()
    {
        health.Damaged -= OnHurt;
        health.Died -= OnDead;
    }


    public void OnHurt(Vector2 vector2)
    {

    }


    public void OnDead()
    {
        Destroy(gameObject);
    }

}
