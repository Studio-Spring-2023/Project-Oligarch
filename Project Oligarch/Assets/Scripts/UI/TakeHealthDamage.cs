using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHealthDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxShield = 100;
    public int currentShield;

    public HealthBar healthBar;
    public ShieldBar shieldBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeShield(5);
        }
    }

    void TakeHealth(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void TakeShield(int damage)
    {
        currentShield -= damage;
        shieldBar.SetShield(currentShield);
    }

    public void TakeDamage(int Damage)
    {
        if(currentShield >= 0)
        {
            currentShield -= Damage;
            shieldBar.SetShield(currentShield);

        }
        else
        {
            currentHealth -= Damage;
            healthBar.SetHealth(currentHealth);
        }
    }
}