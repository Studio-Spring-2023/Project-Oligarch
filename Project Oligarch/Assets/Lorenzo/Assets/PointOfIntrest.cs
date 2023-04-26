using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfIntrest : MonoBehaviour
{
    public float Range;
    private Transform Playertrans;
    private Shop shop;
    void Start()
    {
        Playertrans = GameObject.FindWithTag("Player").transform;
        shop = GameObject.FindWithTag("Manager").GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(InRange());
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

    }

    private void CalcReward()
    {
        int randIndex = Random.Range(0, 100);
        if(randIndex <= 50)
        {
            Debug.Log("SpawnShop");
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
