using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopStall : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] private List<ShopItem> Items = new List<ShopItem>();
    [SerializeField] TextMeshPro priceText;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    public float innerRadius;
    private int minPrice;
    public float growSpeed;
    private bool grow;
    private bool inside;
    void Awake()
    {
        itemName = GameObject.FindWithTag("ItemName").GetComponent<TextMeshProUGUI>(); 
        itemDesc = GameObject.FindWithTag("ItemDesc").GetComponent<TextMeshProUGUI>();
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
        priceText = GetComponentInChildren<TextMeshPro>();
        priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        priceText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Items = shop.ShopPool;
        if(grow)
        {
            PopTextOut();
        }  
        if(inside)
        {
            grow = false;
            priceText.gameObject.SetActive(false);
            priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            priceText.text = FindminPrice().ToString();
            priceText.gameObject.SetActive(true);
            grow = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        float dist = Vector3.Distance(other.transform.position , transform.position);
        if (other.gameObject.CompareTag("Player") && dist < innerRadius)
        {
            inside = true;
        }
        else
        {
            inside = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            grow = false;
            priceText.gameObject.SetActive(false);
            priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            itemName.enabled = false;
            itemDesc.enabled = false;
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


    private void PopTextOut()
    {
        if(priceText.rectTransform.localScale.x < 1f)
        {
            priceText.rectTransform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growSpeed;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, innerRadius);
    }
}
