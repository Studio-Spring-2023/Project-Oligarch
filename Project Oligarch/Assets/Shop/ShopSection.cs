using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSection : MonoBehaviour
{
    public ShopItem CurrItem;
    public Transform HoverPoint;
    private Vector3 startPoint;
    public float HoverHeight;
    public float HoverSpeed;
    private int Price;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] private GameObject Item;
    public float RotateSpeed;
    public float growSpeed;
    private bool grow;
    public float distCheck;
    void Start()
    {
        Price = CurrItem.price;
        Item = Instantiate(CurrItem.DisplayPrefab, HoverPoint.position, Quaternion.identity);
        startPoint = HoverPoint.position;
        priceText = GetComponentInChildren<TextMeshPro>();
        priceText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        priceText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HoverItem();
        SpinItem();
        if(inFront())
        {
            priceText.text = Price.ToString();
            priceText.gameObject.SetActive(true);
            Debug.Log("Hit");
            PopTextOut();
        }
        else
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.forward * distCheck);
    }
}
