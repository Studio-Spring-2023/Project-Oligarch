using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryHandler : MonoBehaviour
{
   public static Dictionary<ItemEnum.Items, int> PlayerItems = new Dictionary<ItemEnum.Items, int>();
    

    public void Awake()
    {
        GenerateEmptyInventory();

        PrintInvetory();
    }
    private void GenerateEmptyInventory()
    {
        for (int i =0; i < ((int)ItemEnum.Items.Max); i++)
        {
            PlayerItems.Add((ItemEnum.Items)i,0);
            
        }
    }
    private void ResetInventory()
    {
        for (int i =0; i < ((int)ItemEnum.Items.Max); i++)
        {
            PlayerItems[(ItemEnum.Items)i] = 0;
            
        }
    }
    private void PrintInvetory()
    {
        foreach (KeyValuePair<ItemEnum.Items, int> pair in PlayerItems)
        {
            Debug.Log($"{pair.Key}: {PlayerItems[pair.Key]}");
        }
    }
    /*
    public void ItemAdded( ItemEnum.Items ItemAdded)
    {
        int count;
        PlayerItems.TryGetValue(key, out count );
        PlayerItems[key] = count + 1;
    }
    
    public void ItemRemove( ItemEnum.Items ItemRemove )
    {
        int count;
        PlayerItems.TryGetValue(key, out count );
        PlayerItems[key] = count - 1;
    }
    */
    public int HowManyItemsDoIHave( ItemEnum.Items itemToCheck )
    {
        return PlayerItems[itemToCheck];
    }
}
