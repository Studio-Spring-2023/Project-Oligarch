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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Bought();
        }
    }

    void Bought()
    {
        Debug.Log("Bought Item");
        Destroy(gameObject);
    }
}
