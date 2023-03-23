using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopStall : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] private List<ShopItem> Items = new List<ShopItem>();
    [SerializeField] TextMeshPro priceText;
    private int minPrice;
    void Awake()
    {
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
        priceText = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Items = shop.ShopPool;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log(FindminPrice());
        }

    }
    private int FindminPrice()
    {
        int total = 999;
        for( int i = 0; i < shop.CurrentItems.Count; i++)
        {
            if(shop.CurrentItems[i].price < total)
            {
                total = shop.CurrentItems[i].price;
            }
        }
        return total;
    }
}
