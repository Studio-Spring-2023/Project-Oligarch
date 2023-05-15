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
    public bool isBarrel;


    //GameObject enemy;
    Collider m_Collider;

    private void Awake ( )
    {
        currentHealth = maxHealth;
        m_Collider = GetComponent<Collider>();
    }

    public void LoseLife(float amount )
    {
        currentHealth -= amount;
        if( currentHealth <= 0f && isBarrel == false)
        {
            Money.Credits += worth;
            StartCoroutine(Die());
        } else if (currentHealth <= 0f && isBarrel == true)
        {
            gameObject.GetComponent<ExplosiveBarrel>().Explode();
        }
    }

    IEnumerator Die()
    {
        MobCore[] scripts = gameObject.GetComponents<MobCore>();
        foreach (MobCore script in scripts)
        {
            script.enabled = false;
            m_Collider.enabled = !m_Collider.enabled;
        }

        enemyAnim.SetInteger("action", death_action_number);
        yield return new WaitForSeconds(death_despawn);
        Destroy(gameObject);
    }
 }
