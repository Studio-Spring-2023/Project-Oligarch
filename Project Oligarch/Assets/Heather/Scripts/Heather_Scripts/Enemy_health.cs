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
        Vector3 sourcePosition = gameObject.transform.position; 
        SoundManager.instance.PlaySound(4, sourcePosition);
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

    public void GainLife(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
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
        this.gameObject.tag = "dead";
        Vector3 sourcePosition = gameObject.transform.position;
        SoundManager.instance.PlaySound(6, sourcePosition);

        enemyAnim.SetInteger("action", death_action_number);
        yield return new WaitForSeconds(death_despawn);
        Destroy(gameObject);
    }
 }
