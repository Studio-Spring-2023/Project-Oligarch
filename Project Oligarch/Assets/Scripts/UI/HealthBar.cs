using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerCore playerCore;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health + playerCore.healthFlatMod * ( 1 + playerCore.healthPercentMod );
    }

    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }

    public void SetShield(int shield)
    {
        slider.value = shield + playerCore.shieldMod;
    }
}