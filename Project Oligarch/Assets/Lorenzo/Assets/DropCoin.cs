using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
     public GameObject CoinObj;
    public float Force;

    // Update is called once per frame
    void Coin()
    {
        Instantiate(CoinObj, transform.position, Quaternion.identity);
        CoinObj.GetComponent<Rigidbody>().AddForce(transform.up * Force);
    }
}
