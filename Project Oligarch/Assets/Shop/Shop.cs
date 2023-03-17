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
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    [SerializeField] GameObject ShopStall;
    [SerializeField] GameObject ShopSection;
    public List<ShopItem> ShopList = new List<ShopItem>();
    public List<GameObject> SectionList = new List<GameObject>();
    [SerializeField]  public Dictionary<Items, Rarity> ShopItems = new Dictionary<Items,Rarity>();
    [Tooltip("Increases shop spawn odds by percentage")]
    public int OddsMod;
    public int ShopCount;

    private void Start()
    {
        ShopCount = SectionList.Count;
        NewShop();
        GenerateShop(transform.position, ShopCount);
        RandomShopRarity();
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

    }

    public void NewShop()//(ShopType rarity)
    {
        for(int i = 0; i < ShopCount; i++)
        {
            SectionList[i] = ShopSection;
        }
        //this should give us new shop odds in the future
    }

    private Rarity RandomShopRarity() //Rarity roll
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

    /*private CommonItems RollCommon()
    {
        CommonItems Item = Random.Range(0, (int)CommonItems.MAX - 1);
        Debug.Log(Item);
        return Item;
    }*/

}
