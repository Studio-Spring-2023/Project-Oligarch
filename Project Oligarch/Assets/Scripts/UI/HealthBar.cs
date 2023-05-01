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
        healthSlider.maxValue = HP.maxHealth;
        healthSlider.value = HP.currentHealth + playerCore.healthFlatMod * ( 1 + playerCore.healthPercentMod );
        ShieldSlider.maxValue = HP.maxShield;
        ShieldSlider.value = HP.currentShield + playerCore.shieldMod;
    }
}