using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeHealthDamage : MonoBehaviour
{
    public static int maxHealth = 100;
    public static int currentHealth;
    public static int maxShield = 100;
    public static int currentShield;
    public static bool dodge = false;

    public HealthBar healthBar;
    public HealthBar shieldbar;


    void Start ()
    {
        currentHealth = maxHealth;

        currentShield = maxShield;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(5);
        }
        GameOver();

    }

    public static void TakeDamage(int Damage)
    {
        TakeHealthDamage.RollDamage();
        if(!TakeHealthDamage.dodge)
        {
            if (currentShield >= 0)
            {
                currentShield -= (Damage - StatMods.damMiti);

            }
            else
            {
                currentHealth -= (Damage - StatMods.damMiti);
            }
            TakeHealthDamage.dodge = false;
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

    public static void RollDamage()
    {
        int rand = Random.Range(1, 100);
        if(rand < StatMods.mitiChance)
        {
            TakeHealthDamage.dodge = true;
            Debug.Log("Dodged");
        }
    }
}