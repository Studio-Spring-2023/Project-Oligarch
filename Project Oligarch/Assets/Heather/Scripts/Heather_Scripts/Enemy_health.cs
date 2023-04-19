using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;

    private void Awake ( )
    {
        currentHealth = maxHealth;
    }

    public void LoseLife(float amount )
    {
        currentHealth -= amount;

        if( currentHealth <= 0f )
        {
            Destroy ( gameObject );
        }
    }
}
