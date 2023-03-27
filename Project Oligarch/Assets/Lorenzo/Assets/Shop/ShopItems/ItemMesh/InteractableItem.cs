using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Interactable
{
    public enum Items 
    {
        MREPack,
        TacticalGloves,
        SharpDarts,
        BTCleaver,
        CleanCuts,
        KiteShield,
        HitList,
        Mom,
        Envy,
        HealingCrystal,
        Voodoo
    }
    public ShopSection Section;
    // Update is called once per frame

    public override void InteractedWith()
    {
        Section.Bought();

    }


}
