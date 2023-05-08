using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeHealthDamage : MonoBehaviour
{
    public int maxHealth = 100;
    public static int currentHealth;
    public int maxShield = 100;
    public static int currentShield;

    public int healthPercentMod;
    public int healthFlatMod;
    public int shieldMod;

    public HealthBar healthBar;
    public HealthBar shieldbar;


    void Start ()
    {
        currentHealth = maxHealth;

        currentShield = maxShield;

    }

    void Update()
    {
        currentHealth = maxHealth * ( 1 + healthPercentMod ) + healthFlatMod;
        currentShield = maxShield + shieldMod;

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(5);
        }
        GameOver();

    }

    public static void TakeDamage(int Damage)
    {
        if(currentShield >= 0)
        {
            currentShield -= Damage;

        }
        else
        {
            currentHealth -= Damage;
        }
    }

    public void GameOver()
    {
        if(currentHealth <= 0) 
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
        }
            
    }
}