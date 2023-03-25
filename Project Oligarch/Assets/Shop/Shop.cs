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
        Legendary,
        Null
    }

    [SerializeField] GameObject ShopStall;
    [SerializeField] GameObject ShopSection;
    public List<ShopItem> ShopList = new List<ShopItem>();
    [SerializeField]  private List<ShopItem> CommonItems = new List<ShopItem>();
    [SerializeField]  private List<ShopItem> UnCommonItems = new List<ShopItem>();
    [SerializeField]  private List<ShopItem> RareItems = new List<ShopItem>();
    [SerializeField]  private List<ShopItem> LegendaryItems = new List<ShopItem>();
    public List<GameObject> ObjSectionList = new List<GameObject>();
    public List<ShopItem> CurrentItems = new List<ShopItem>();
    [SerializeField] private List<ShopSection> StallList = new List<ShopSection>();

   [SerializeField]  public List<ShopItem> ShopPool = new List<ShopItem>();
    //[SerializeField]  public Dictionary<Items, ShopRarity> ShopItems = new Dictionary<Items,ShopRarity>();
    [Tooltip("Increases shop spawn odds by percentage")]
    public int OddsMod;
    public int ShopCount;

    private void Start()
    {
        ShopCount = ObjSectionList.Count;
        AssignShopItems();
        GenerateShopPool();
        GenerateShop(transform.position, ShopCount);
        NewShop(3);   
    }


    public void GenerateShop(Vector3 shopPosition, int shopSize)
    {
        //GenerateShopPool();

        float off = 0;
        GameObject currStall;
        Instantiate(ShopStall, shopPosition, Quaternion.identity);
        for (int i = 0; i < shopSize; i++)
        {
            Vector3 Offset = shopPosition + (transform.right * off);
            currStall = Instantiate(ShopSection, Offset, Quaternion.identity);
            ObjSectionList[i] = currStall;
            off++;
        }
        //NewShop(3);
        
    }
    private void GenerateShopPool()
    {
        ShopRarity rare = RandomShopRarity();
        for(int i = 0; i < 8; i++)
        {
            ShopPool.Add(RollRarity(rare));
        }
    }
    
    public void NewShop(int count)//(ShopType ShopRarity)
    {
        int randIndex;
        for(int i = 0; i < count; i++)
        {
            StallList[i] = ObjSectionList[i].GetComponent<ShopSection>();
            StallList[i].placeInList = i;
        }
        for(int i = 0; i < StallList.Count; i++)
        {
            randIndex = Random.Range(1,ShopPool.Count);
            StallList[i].CurrItem = ShopPool[randIndex];
            CurrentItems.Add(ShopPool[randIndex]);
            ShopPool.RemoveAt(randIndex);
            Debug.Log(StallList[i].CurrItem);
        }
        
    }

    public void ReplaceItem(int stallNum)
    {
        if(ShopPool.Count <= 0)
        {
            StallList[stallNum].DestroySelf();
            return;
        }
        int randIndex;
        randIndex = Random.Range(1, ShopPool.Count);
        StallList[stallNum].CurrItem = ShopPool[randIndex];
        CurrentItems.Add(ShopPool[randIndex]);
        ShopPool.RemoveAt(randIndex);
        Debug.Log(StallList[stallNum].CurrItem);
    }

#region InitalizeShop

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


#endregion

#region RarityRolls 
    private ShopRarity RandomShopRarity() //ShopRarity roll
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
        if(Odds > 99)
        {
            ShopRarity = ShopRarity.Legendary;
        }
        Debug.Log(Odds);
        Debug.Log(ShopRarity);

        return ShopRarity;
    }

    public ShopItem RollRarity(ShopRarity shopRarity)
    {
        float Odds = Random.Range(0f,100f);
        switch(shopRarity)
        {
            case ShopRarity.Common:
            {
                if(Odds <= 80f )
                {
                    return RollItem(ShopRarity.Common);
                }
                else if(Odds >80f && Odds <= 94.5f)
                {
                    return RollItem(ShopRarity.Uncommon);
                }
                else if(Odds > 94.5f && Odds <= 99.5f)
                {
                    return RollItem(ShopRarity.Rare);
                }
                else if(Odds > 99.5f)
                {
                    return RollItem(ShopRarity.Legendary);
                }
                break;
            }
            case ShopRarity.Uncommon:
            {
                if(Odds <= 30f )
                {
                    return RollItem(ShopRarity.Common);
                }
                else if(Odds > 30f && Odds <= 90f)
                {
                    return RollItem(ShopRarity.Uncommon);
                }
                else if(Odds > 90f && Odds <= 99f)
                {
                    return RollItem(ShopRarity.Rare);
                }
                else if(Odds > 99f)
                {
                    return RollItem(ShopRarity.Legendary);
                }
                break;
            }
            case ShopRarity.Rare:
            {
                if(Odds <= 15f )
                {
                    return RollItem(ShopRarity.Common);
                }
                else if(Odds > 15f && Odds <= 45f)
                {
                    return RollItem(ShopRarity.Uncommon);
                }
                else if(Odds > 45f && Odds <= 95f)
                {
                    return RollItem(ShopRarity.Rare);
                }
                else if(Odds > 95f)
                {
                    return RollItem(ShopRarity.Legendary);
                }
                break;
            }
            case ShopRarity.Legendary:
            {
                if(Odds <= 0.5f )
                {
                    return RollItem(ShopRarity.Common);
                }
                else if(Odds > 0.5f && Odds <= 35.5f)
                {
                    return RollItem(ShopRarity.Uncommon);
                }
                else if(Odds > 35.5f && Odds <= 80.5f)
                {
                    return RollItem(ShopRarity.Rare);
                }
                else if(Odds > 80.5f)
                {
                    return RollItem(ShopRarity.Legendary);
                }
                break;
            }
            default:
            {
                Debug.Log("Something went wrong :(");
                break;
            }   
                
        }
        return ShopList[0];
    }

    public ShopItem RollItem(ShopRarity ItemRarity)
    {
        int randIndex;

            switch(ItemRarity)
            {
                case ShopRarity.Common:
                {
                    randIndex = Random.Range(1,CommonItems.Count);
                    return CommonItems[randIndex];
                }
                case ShopRarity.Uncommon:
                {
                    randIndex = Random.Range(0,UnCommonItems.Count);
                    return UnCommonItems[randIndex];
                }                   
                case ShopRarity.Rare:
                {
                    randIndex = Random.Range(0,RareItems.Count);
                    return RareItems[randIndex];
                }
                case ShopRarity.Legendary:
                {
                    randIndex = Random.Range(0,LegendaryItems.Count);
                    return LegendaryItems[randIndex]; 
                }
                default:
                {
                    Debug.Log("Something went wrong :(");
                    break;
                }
            }
            return ShopList[0];
            

    }
#endregion


}
