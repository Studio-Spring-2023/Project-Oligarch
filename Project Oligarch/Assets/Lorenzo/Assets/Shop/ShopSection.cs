using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSection : MonoBehaviour
{
    public ShopItem CurrItem;
    public Shop shop;
    public Transform HoverPoint;
    private Vector3 startPoint;
    public float HoverHeight;
    public float HoverSpeed;
    [HideInInspector]
    public int Price;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] private GameObject Item;
    [SerializeField] Money money;
    public float RotateSpeed;
    public float growSpeed;
    private bool grow;
    public float distCheck;
    public int placeInList;
    
    void Start()
    {
        itemName = GameObject.FindWithTag("ItemName").GetComponent<TextMeshProUGUI>(); //yeah I know this is pretty bad
        itemDesc = GameObject.FindWithTag("ItemDesc").GetComponent<TextMeshProUGUI>();
        money = GameObject.FindWithTag("Manager").GetComponent<Money>();
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
        Price = CurrItem.price;
        Item = Instantiate(CurrItem.DisplayPrefab, HoverPoint.position, Quaternion.identity);
        Item.GetComponent<InteractableItem>().Section = this;
        startPoint = HoverPoint.position;
        priceText = GetComponentInChildren<TextMeshPro>();
        priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        priceText.gameObject.SetActive(false);
        itemName.enabled = false;
        itemDesc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Item == null && shop.ShopPool.Count > 0)
        {
            shop.ReplaceItem(placeInList);
            Price = CurrItem.price;
            Item = Instantiate(CurrItem.DisplayPrefab, HoverPoint.position, Quaternion.identity);
            Item.GetComponent<InteractableItem>().Section = this;
        }
        else if (Item == null)
        {
            DestroySelf();
        }
        if(Item != null)
        {
            HoverItem();
            SpinItem();
        }
        if(inFront() && priceText != null)
        {
            priceText.text = Price.ToString();
            priceText.gameObject.SetActive(true);
            PopTextOut();
            Descriptions();
            itemName.enabled = true;
            itemDesc.enabled = true;
        }
        else if(priceText != null)
        {
            priceText.gameObject.SetActive(false);
            priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    private void HoverItem()
    {
        float sinWave = Mathf.Sin(Time.time * HoverSpeed) * HoverHeight;
        Item.transform.position =  startPoint + transform.up * Mathf.Sin(Time.time * HoverSpeed) * HoverHeight;
    }
    private void SpinItem()
    {
        Item.transform.Rotate(0,RotateSpeed * Time.deltaTime,0);
    }

    private bool inFront()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.forward, out hit, distCheck))
            {
                if(hit.transform.tag == "Player")
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
        return false;
    }
    private void PopTextOut()
    {
        if (priceText.rectTransform.localScale.x < 1f)
        {
            priceText.rectTransform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growSpeed;
        }
    }
    private void Descriptions()
    {
        itemDesc.text = CurrItem.Description;
        itemName.text = CurrItem.DisplayName;

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.forward * distCheck);
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void Bought()
    {
        if(Money.Credits >= Price)
        {
            ItemManager.Instance.AddItemToInventory(CurrItem.data);
            Destroy(Item);
            Money.Credits -= Price;
        }
        
    }
}
