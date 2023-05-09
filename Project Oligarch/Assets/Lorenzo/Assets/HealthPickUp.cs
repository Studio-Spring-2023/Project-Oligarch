using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int amount;
    private PlayerCore player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCore>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Core.Health += amount;
            Destroy(gameObject);
        }
    }
}
