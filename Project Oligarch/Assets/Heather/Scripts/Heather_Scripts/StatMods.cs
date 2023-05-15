using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMods : MonoBehaviour
{
    ItemManager itemManager;
    [SerializeField] ItemManager items;
    public static int coolDownMod;
    public static float healthPercentMod;
    public static int healthFlatMod;
    public static int healthRegen;
    public static int secForRegen;
    public static int shieldMod;
    public static int damMiti; 

    public static float damMod;
    public static float atkSpeedMod;
    public static float moveMod;
    public static float jumpMod;
    public static float mitiChance; 
    public static float lifeSteal;

    public void Update()
    {
        coolDownMod = (int)items.FlatCooldownReduction;
        healthPercentMod = items.PercentHealthBonus;
        healthFlatMod = (int)items.FlatHealthBonus;
        shieldMod = (int)items.FlatMaxShield;
        damMiti = (int)items.FlatDamageMitigation;
        damMod = items.PercentDamageBonus;
        lifeSteal = items.PercentLifeSteal;
        atkSpeedMod = items.PercentAttackSpeedBonus;
        moveMod = items.PercentMovespeedBonus;
        jumpMod = items.JumpChargeBonus;
        mitiChance = items.PercentMitigationChance;
    }

    public static void StatsMod ( )
    {
        TakeHealthDamage.currentHealth = ( TakeHealthDamage.maxHealth * (int)( 1 + healthPercentMod ) ) + healthFlatMod + ( healthRegen * secForRegen );
        TakeHealthDamage.currentShield = TakeHealthDamage.maxShield + shieldMod;
        

        projectiles.Dam = projectiles.Dam * ( 1 + damMod );
        projectiles.TimeBetweenShooting = projectiles.TimeBetweenShooting - coolDownMod * ( 1 + atkSpeedMod );

        HomingMissile.Dam = projectiles.Dam * ( 1 + damMod );
        HomingMissile.force = HomingMissile.force * ( 1 + atkSpeedMod );

        PlayerCore.JumpForce = PlayerCore.JumpForce * jumpMod;
        PlayerCore.MoveSpeed = PlayerCore.MoveSpeed * ( 1 + moveMod );

        projectiles.LifeSteal = lifeSteal;
    }
}
