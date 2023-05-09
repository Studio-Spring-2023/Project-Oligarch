using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider ShieldSlider;
    public PlayerCore playerCore;
    public TakeHealthDamage HP;


    public void Start()
    {
        HP = GameObject.FindWithTag("Player").GetComponent<TakeHealthDamage>();
        playerCore = GameObject.FindWithTag("Player").GetComponent<PlayerCore>();
    }

    private void Update()
    {
        if(healthSlider != null && ShieldSlider != null)
        {
            healthSlider.maxValue = TakeHealthDamage.maxHealth;
            healthSlider.value = TakeHealthDamage.currentHealth;
            ShieldSlider.maxValue = TakeHealthDamage.maxShield;
            ShieldSlider.value = TakeHealthDamage.currentShield;
        }

    }
}