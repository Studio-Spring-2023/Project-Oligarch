using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSection : MonoBehaviour
{
    public ShopItem CurrItem;
    public Transform HoverPoint;
    private Vector3 startPoint;
    public float HoverHeight;
    public float HoverSpeed;
    private float Price;
    [SerializeField] private GameObject Item;
    void Start()
    {
        Price = CurrItem.price;
        Item = Instantiate(CurrItem.DisplayPrefab, HoverPoint.position, Quaternion.identity);
        startPoint = HoverPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        HoverItem();
    }

    private void HoverItem()
    {
        float sinWave = Mathf.Sin(Time.time * HoverSpeed) * HoverHeight;
        Item.transform.position =  startPoint + transform.up * Mathf.Sin(Time.time * HoverSpeed) * HoverHeight;
    }
}
