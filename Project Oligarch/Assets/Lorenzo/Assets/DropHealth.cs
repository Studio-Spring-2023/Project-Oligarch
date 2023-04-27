using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHealth : MonoBehaviour
{
    public GameObject HealthObj;
    public float Force;

    // Update is called once per frame
    public void Health(int amount, Vector3 pos)
    {
        GameObject heart = Instantiate(HealthObj, pos, Quaternion.identity);
        heart.GetComponent<HealthPickUp>().amount = amount;
        HealthObj.GetComponent<Rigidbody>().AddForce(transform.up * Force);
    }
}
