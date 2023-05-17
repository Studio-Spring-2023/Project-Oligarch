using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfIntrest : MonoBehaviour
{
    public float Range;
    private Transform Playertrans;
    private Shop shop;
    public SpawnEnemies spawnenemies;
    public GameObject Section;
    public GameObject TP;
    public Transform Point2;
    private DropCoin coin;
    private DropHealth health;
    private bool startedPOI = false;
    public bool Boss = false;
    public bool completed = false;
    void Start()
    {
        Playertrans = GameObject.FindWithTag("Player").transform;
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
        coin = GameObject.FindWithTag("Manager").GetComponent<DropCoin>();
        health = GameObject.FindWithTag("Manager").GetComponent<DropHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange() && !startedPOI)
        {
            StartEncounter();
            startedPOI = true;
        }

        if(spawnenemies.Finish == true && startedPOI && !Boss)
        {
            CalcReward();
            completed = true;
        }
        else if (spawnenemies.Finish == true && startedPOI && Boss)
        {
            BossEncounter();
        }
    }

    private bool InRange()
    {
        if(Vector3.Distance(transform.position, Playertrans.position) < Range )
        {
            return true;
        }
        else
            return false;
    }

    private void StartEncounter()
    {
        
        spawnenemies.SpawnEnemiesFunc();
    }

    private void CalcReward()
    {
        spawnenemies.Finish = false;
        int randIndex = Random.Range(0, 100);
        if(randIndex <= 50)
        {
           shop.GenerateShop(transform.position, 3);
        }
        else if (randIndex > 50)
        {
            AltReward();
        }
    }

    private void AltReward()
    {
        int randIndex = Random.Range(0, 100);
        if(randIndex <= 40)
        {
            coin.Coin(50);
            health.Health(50, Point2.position);
        }
        else if( randIndex > 40)
        {
            //ItemRarity();
            shop.GenerateShop(transform.position, 1);
        }
    }

    void ItemRarity()
    {
        int randIndex = Random.Range(0, 100);
        if(randIndex <= 40)
        {
            shop.SingleSection(Shop.ShopRarity.Common, Section, transform.position);
        }
        else if(randIndex >= 40 && randIndex < 70)
        {
            shop.SingleSection(Shop.ShopRarity.Uncommon, Section, transform.position);
        }
        else if (randIndex >= 70 && randIndex < 90)
        {
            shop.SingleSection(Shop.ShopRarity.Rare, Section, transform.position);
        }
        else if(randIndex >= 90)
        {
            shop.SingleSection(Shop.ShopRarity.Legendary, Section, transform.position);
        }
    }

    private void BossEncounter()
    {
        Debug.Log("Beat Boss");
        spawnenemies.Finish = false;
        Instantiate(TP, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Range);  
    }
}
