using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnum : MonoBehaviour
{

    public enum Items
    {
        //common
        MREPack,
        SyntheticWeave,
        TacticalGloves,
        SharpDarts,
        SteelToedBoots,
        FMJBullet,
        FleetingSupercharger,
        BrokenSupercharger,
        GlassSpurs,
        BrokenSpurs,
        QuickReloadBandolier,
        WornWhetstone,
        EmergencyMedkit,
        LuckyTrinket,
        Adrenaline,
        IonShielding,
        SteelTonfa,
        _4XBinoculars,
        HiddenKnife,
        ExposedWires,
        OverchargedTeslaCoil,
        HoldingGrudges,
        //end common ^

        //uncommon
        BloodThirstyCleaver,
        Soda,
        AlAssistedTreatment,
        ExecutionersAxe,
        OpeningGambit,
        EMP,
        CleanTuts,
        LingeringHeart,
        EngineExhaust,
        ChargedBattery,
        ThrusterPack,
        Aerodynamics,
        SpikyShield,
        KiteShield,
        BackupArsenal,
        PocketKnife,
        DoubleTime,
        HealthInAJar,
        BlowTorch,
        BatteryPack,
        InsidersInfo,
        //end uncommon ^

        //rare
        CounterWeight,
        Envy,
        Protien,
        DeathTaxes,
        InvertedHourglass,
        AutomatedReloading,
        Speedometer,
        DuctTapedGrenades,
        BarrierSiphon,
        LargePockets,
        MomsCreditCard,
        HitList,
        //end rare ^

        //legendary
        MiniatureWindmill,
        CloakingArmband,
        LotteryWheel,
        TargetingSystems,
        MintCondintionBullseye,
        ScrapMechanincsWrench,
        HealingCrystals,
        VoodooCurse,
        //end legendary ^

        Max
    }
    
    
        public Items item;
    
}
