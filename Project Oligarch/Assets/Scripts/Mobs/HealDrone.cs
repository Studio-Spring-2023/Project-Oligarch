using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealDrone : MonoBehaviour
{
    public GameObject[] Allies;
    public GameObject NearestAlly;
    float distance;
    float nearestDistance;

    public Transform target;
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        Allies = GameObject.FindGameObjectsWithTag("enemies");
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);
    }
}
