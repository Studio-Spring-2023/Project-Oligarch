using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
     public GameObject CoinObj;
    public float Force;

    // Update is called once per frame
    public void Coin(int amount)
    {
        GameObject coin = Instantiate(CoinObj, transform.position, Quaternion.identity);
        coin.GetComponent<Coin>().worth = amount;
        CoinObj.GetComponent<Rigidbody>().AddForce(transform.up * Force);
    }
}
