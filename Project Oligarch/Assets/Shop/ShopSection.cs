using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSection : MonoBehaviour
{
    public ShopItem CurrItem;
    private float Price;
    void Start()
    {
        Price = CurrItem.price;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
