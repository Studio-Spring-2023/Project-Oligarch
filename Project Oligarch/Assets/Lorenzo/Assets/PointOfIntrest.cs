using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfIntrest : MonoBehaviour
{
    public float Range;
    private Transform Playertrans;
    private Shop shop;
    public SpawnEnemies spawnenemies;
    private bool startedPOI = false;
    void Start()
    {
        Playertrans = GameObject.FindWithTag("Player").transform;
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange() && !startedPOI)
        {
            StartEncounter();
        }
        if(spawnenemies.Finish == true && startedPOI)
        {
            CalcReward();
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
        startedPOI = true;
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
        Debug.Log("randomly choose a reward");
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Range);  
    }
}
