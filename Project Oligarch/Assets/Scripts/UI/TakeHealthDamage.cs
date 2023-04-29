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
    public HealthBar shieldbar;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentShield = maxShield;
        shieldbar.SetMaxShield(maxShield);
    }

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
        shieldbar.SetShield(currentShield);
    }

    public void TakeDamage(int Damage)
    {
        if(currentShield >= 0)
        {
            currentShield -= Damage;
            shieldbar.SetShield(currentShield);

        }
        else
        {
            currentHealth -= Damage;
            healthBar.SetHealth(currentHealth);
        }
    }

    public void GameOver()
    {
        if(currentHealth <= 0) 
        {
            Debug.Log("GameOver");
        }
            
    }
}