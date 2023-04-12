using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int worth;
    private Money money;
    void Awake()
    {
        money = GameObject.FindWithTag("Manager").GetComponent<Money>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        money.Credits += worth;
        Destroy(gameObject);
    }
}
