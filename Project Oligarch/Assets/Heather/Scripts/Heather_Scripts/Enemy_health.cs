using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public int maxHealth = 30;
    public int currentHealth;

    private void Awake ( )
    {
        currentHealth = maxHealth;
    }

    public void LoseLife(int amount )
    {
        currentHealth -= amount;

        if( currentHealth <= 0 )
        {
            Destroy ( gameObject );
        }
    }
}
