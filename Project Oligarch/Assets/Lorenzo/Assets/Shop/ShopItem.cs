using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopItem : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
    public ItemData data;
    public Rarity rarity;
    public GameObject DisplayPrefab;
    public string DisplayName;
    public int price;
    public string Description;
}
