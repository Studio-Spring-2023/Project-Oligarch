using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        //legendary
        MiniatureWindmill,
        CloakingArmband,
        LotteryWheel,
        TargetingSystems,
        MintCondintionBullseye,
        ScrapMechanincsWrench,
        HealingCrystals,
        VoodooCurse, 
        Max
    }

public class InventoryHandler : MonoBehaviour
{
   public static Dictionary<Items, int> PlayerItems = new Dictionary<Items, int>;

    public void Awake()
    {
        GenerateEmptyInventory();

        PrintInvetory();
    }
    private void GenerateEmptyInventory()
    {
        for (int i =0; i < ((int)Items.Max); i++)
        {
            PlayerItems.Add((Items)i,0);
            
        }
    }
    private void void ResetInventory()
    {
        for (int i =0; i < ((int)Items.Max); i++)
        {
            PlayerItems[(Items)i] = 0;
            
        }
    }
    private void PrintInvetory()
    {
        foreach (KeyValuePair<Items, int> pair in PlayerItems)
        {
            Debug.Log($"{pair.Key}: {PlayerItems[pair.Key]}");
        }
    }
    public void ItemAdded(Items ItemAdded)
    {

    }
    
    public void ItemRemove(Items ItemRemove)
    {

    }
    public int HowManyItemsDoIHave(Items itemToCheck)
    {
        return PlayerItems[itemToCheck];
    }
}
