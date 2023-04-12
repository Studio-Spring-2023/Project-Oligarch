using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetaShop : Interactable
{
    [SerializeField] Image Menu;
    [SerializeField] Money money;
    public List<MetaUpgrade> Upgrades = new List<MetaUpgrade>();
    [SerializeField] Image ItemImage1;
    [SerializeField] Image ItemImage2;
    [SerializeField] TextMeshProUGUI ItemDesc1;
    [SerializeField] TextMeshProUGUI ItemDesc2;
    [SerializeField] TextMeshProUGUI ItemName1;
    [SerializeField] TextMeshProUGUI ItemName2;
    [SerializeField] TextMeshProUGUI ItemLVL1;
    [SerializeField] TextMeshProUGUI ItemLVL2;
    [SerializeField] TextMeshProUGUI Itemprice1;
    [SerializeField] TextMeshProUGUI Itemprice2;

    public int currIndex1;
    public int currIndex2;
    public float growSpeed;
    private bool grow;
    void Start()
    {
        Menu.gameObject.SetActive(false);
        currIndex1 = 0;
        currIndex2 = 1;
    }
    private void Update()
    {
        if(grow)
        {
            if (Menu.rectTransform.localScale.x < 1f)
            {
                Menu.rectTransform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growSpeed;
            }
            else
            {
                grow = false;
            }
        }
        ItemImage1.sprite = Upgrades[currIndex1].Sprite1;
        ItemImage2.sprite = Upgrades[currIndex2].Sprite1;
        ItemDesc1.text = Upgrades[currIndex1].Description1;
        ItemDesc2.text = Upgrades[currIndex2].Description1;
        ItemName1.text = Upgrades[currIndex1].Name;
        ItemName2.text = Upgrades[currIndex2].Name;
        ItemLVL1.text = MetaManager.MetaDict[Upgrades[currIndex1].Name].ToString();
        ItemLVL2.text = MetaManager.MetaDict[Upgrades[currIndex2].Name].ToString();
        if(MetaManager.MetaDict[Upgrades[currIndex1].Name] < 1)
        {
            Itemprice1.text = Upgrades[currIndex1].price1.ToString();
        }
        else if(MetaManager.MetaDict[Upgrades[currIndex1].Name] >= 1)
        {
            Itemprice1.text = Upgrades[currIndex1].price2.ToString();
        }

        if (MetaManager.MetaDict[Upgrades[currIndex2].Name] < 1)
        {
            Itemprice2.text = Upgrades[currIndex2].price1.ToString();
        }
        else if (MetaManager.MetaDict[Upgrades[currIndex2].Name] >= 1)
        {
            Itemprice2.text = Upgrades[currIndex2].price2.ToString();
        }
    }

    // Update is called once per frame
    public override void InteractedWith()
    {
        Debug.Log("Opened");
        OpenShop();
    }

    public void OpenShop()
    {
        Menu.gameObject.SetActive(true);
        grow = true;
    }

    public void RightButton()
    {
        Debug.Log("right");
        if(currIndex1 < 4)
        {
            currIndex1 += 2; //yeah I know this is bad 
            currIndex2 += 2;
        }
        else if(currIndex1 >= 4)
        {
            currIndex1 -= 4; //yes this is even worse
            currIndex2 -= 4;
        }
    }
    public void LeftButton()
    {
        if(currIndex1 > 0)
        {
            currIndex1 -= 2;
            currIndex2 -= 2;
        }
        else if(currIndex1 <= 0)
        {
            currIndex1 += 4;
            currIndex2 += 4;
        }
    }
    public void CloseShop()
    {
        Menu.gameObject.SetActive(false);
        Menu.rectTransform.localScale = Menu.rectTransform.localScale/3f;
    }
    public void BuyLeft()
    {
        if (Upgrades[currIndex1].price1 <= money.Credits && MetaManager.MetaDict[Upgrades[currIndex1].Name] < 1)
        {
            MetaManager.MetaDict[Upgrades[currIndex1].Name] += 1;
            money.Credits -= Upgrades[currIndex1].price1;
        }

        else if (Upgrades[currIndex1].price2 <= money.Credits && MetaManager.MetaDict[Upgrades[currIndex1].Name] < 2)
        {
            MetaManager.MetaDict[Upgrades[currIndex1].Name] += 1;
            money.Credits -= Upgrades[currIndex1].price2;
        }
    }
    public void BuyRight()
    {
        if (Upgrades[currIndex2].price1 <= money.Credits && MetaManager.MetaDict[Upgrades[currIndex2].Name] < 1)
        {
            MetaManager.MetaDict[Upgrades[currIndex2].Name] += 1;
            money.Credits -= Upgrades[currIndex2].price1;
        }

        else if (Upgrades[currIndex2].price2 <= money.Credits && MetaManager.MetaDict[Upgrades[currIndex2].Name] < 2)
        {
            MetaManager.MetaDict[Upgrades[currIndex2].Name] += 1;
            money.Credits -= Upgrades[currIndex2].price2;
        }
    }


}
