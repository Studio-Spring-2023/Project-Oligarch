using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;
    public int worth;
    //public Money money;

    private void Awake ( )
    {
        currentHealth = maxHealth;
    }

    public void LoseLife(float amount )
    {
        currentHealth -= amount;
        if( currentHealth <= 0f )
        {
            Money.Credits += worth;
            Destroy ( gameObject );
        }
    }
}
