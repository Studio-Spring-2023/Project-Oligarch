using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
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
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Section.Bought();
        }
    }


}
