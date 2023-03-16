using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public enum CommonItems
    {
        MREPack,
        TacticalGloves,
        SharpDarts,
        MAX
    }
    public enum UncommonItems
    {
        BTCleaver,
        CleanCuts,
        KiteShield,
        MAX
    }
    public enum RareItems
    {
        HitList,
        Mom,
        Envy,
        MAX
    }
    public enum LegendaryItems
    {
        HealingCrystal,
        Voodoo,
        MAX
    }
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    [SerializeField] GameObject ShopStall;
    [SerializeField] GameObject ShopSection;
    public List<GameObject> ShopList = new List<GameObject>();
    [Tooltip("Increases shop spawn odds by percentage")]
    public int OddsMod;
    public int ShopCount;

    private void Start()
    {
        ShopCount = ShopList.Count;
        NewShop();
        GenerateShop(transform.position, ShopCount);
        RandomShopRarity();
    }


    public void GenerateShop(Vector3 shopPosition, int shopSize)
    {
        //Give i shopSection item info with item + price
        //Instantiate(shopPrefab at shopPosition);
        float off = 0;
        Instantiate(ShopStall, shopPosition, Quaternion.identity);
        for (int i = 0; i < ShopList.Count; i++)
        {
            Vector3 Offset = shopPosition + (transform.right * off);
            Instantiate(ShopList[i], Offset, Quaternion.identity);
            off++;
        }
        NewShop();
        
    }

    //shop items spin and float using sign wave

    public void NewShop()//(ShopType rarity)
    {
        for(int i = 0; i < ShopCount; i++)
        {
            ShopList[i] = ShopSection;
        }
        //this should give us new shop odds in the future
    }

    private Rarity RandomShopRarity()
    {
        int Odds = Random.Range(0,100) + OddsMod;
        Rarity rarity = Rarity.Common;

        if(Odds <= 75 )
        {
            rarity = Rarity.Common;
        }
        if(Odds > 75 && Odds <= 94)
        {
            rarity = Rarity.Uncommon;
        }
        if(Odds > 94 && Odds <= 99)
        {
            rarity = Rarity.Rare;
        }
        if(Odds == 100)
        {
            rarity = Rarity.Legendary;
        }
        Debug.Log(Odds);
        Debug.Log(rarity);

        return rarity;
    }

}
