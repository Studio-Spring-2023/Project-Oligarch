using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;
    public int worth;
    //public Money money;

    //animator controller 
    public Animator enemyAnim;
    public float death_despawn = 3;
    public int death_action_number = 2;
   


    //GameObject enemy;


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
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        MobCore[] scripts = gameObject.GetComponents<MobCore>();
        foreach (MobCore script in scripts)
        {
            script.enabled = false;
        }

        enemyAnim.SetInteger("action", death_action_number);
        yield return new WaitForSeconds(death_despawn);
        Destroy(gameObject);
    }
 }
