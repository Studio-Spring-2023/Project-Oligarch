using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public enum Items //this is just for clarity rn
    {
        MREPack = 0,
        TacticalGloves = 0,
        SharpDarts = 0,
        BTCleaver = 1,
        CleanCuts = 1,
        KiteShield = 1,
        HitList = 2,
        Mom = 2,
        Envy = 2,        
        HealingCrystal = 3,
        Voodoo = 3,
        MAX
    }
    public enum ShopRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    [SerializeField] GameObject ShopStall;
    [SerializeField] GameObject ShopSection;
    public List<ShopItem> ShopList = new List<ShopItem>();
   [SerializeField]  private List<ShopItem> CommonItems = new List<ShopItem>();
   [SerializeField]  private List<ShopItem> UnCommonItems = new List<ShopItem>();
   [SerializeField]  private List<ShopItem> RareItems = new List<ShopItem>();
   [SerializeField]  private List<ShopItem> LegendaryItems = new List<ShopItem>();
    public List<GameObject> SectionList = new List<GameObject>();
    //[SerializeField]  public Dictionary<Items, ShopRarity> ShopItems = new Dictionary<Items,ShopRarity>();
    [Tooltip("Increases shop spawn odds by percentage")]
    public int OddsMod;
    public int ShopCount;

    private void Start()
    {
        ShopCount = SectionList.Count;
        NewShop();
        GenerateShop(transform.position, ShopCount);
        RandomShopShopRarity();
        AssignShopItems();
        //RollCommon();
    }


    public void GenerateShop(Vector3 shopPosition, int shopSize)
    {
        //Give i shopSection item info with item + price
        //Instantiate(shopPrefab at shopPosition);
        float off = 0;
        Instantiate(ShopStall, shopPosition, Quaternion.identity);
        for (int i = 0; i < SectionList.Count; i++)
        {
            Vector3 Offset = shopPosition + (transform.right * off);
            Instantiate(SectionList[i], Offset, Quaternion.identity);
            off++;
        }
        NewShop();
        
    }

    //shop items spin and float using sign wave

    private void AssignShopItems()
    {
        foreach (ShopItem i in ShopList)
        {
            if(i.rarity == ShopItem.Rarity.Common)
            {
                CommonItems.Add(i);
            }
            if(i.rarity == ShopItem.Rarity.Uncommon)
            {
                UnCommonItems.Add(i);
            }
            if(i.rarity == ShopItem.Rarity.Rare)
            {
                RareItems.Add(i);
            }
            if(i.rarity == ShopItem.Rarity.Legendary)
            {
                LegendaryItems.Add(i);
            }
        }
    }

    public void NewShop()//(ShopType ShopRarity)
    {
        for(int i = 0; i < ShopCount; i++)
        {
            SectionList[i] = ShopSection;
        }
        //this should give us new shop odds in the future
    }

    private ShopRarity RandomShopShopRarity() //ShopRarity roll
    {
        int Odds = Random.Range(0,100) + OddsMod;
        ShopRarity ShopRarity = ShopRarity.Common;

        if(Odds <= 75 )
        {
            ShopRarity = ShopRarity.Common;
        }
        if(Odds > 75 && Odds <= 94)
        {
            ShopRarity = ShopRarity.Uncommon;
        }
        if(Odds > 94 && Odds <= 99)
        {
            ShopRarity = ShopRarity.Rare;
        }
        if(Odds == 100)
        {
            ShopRarity = ShopRarity.Legendary;
        }
        Debug.Log(Odds);
        Debug.Log(ShopRarity);

        return ShopRarity;
    }

    /*private ShopItems RollCommon()
    {
        if(Odds <= 75 )
        {
            ShopRarity = ShopRarity.Common;
        }
        if(Odds > 75 && Odds <= 94)
        {
            ShopRarity = ShopRarity.Uncommon;
        }
        if(Odds > 94 && Odds <= 99)
        {
            ShopRarity = ShopRarity.Rare;
        }
        if(Odds == 100)
        {
            ShopRarity = ShopRarity.Legendary;
        }
    }*/

}
