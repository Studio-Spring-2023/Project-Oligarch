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

    public float healAmount = .3f;
    public float radius = 10f;

    NavMeshAgent nav;
    Collider m_Collider;
    public bool cd = false;



    // Start is called before the first frame update
    void Start()
    {
        NearestAlly = GameObject.FindGameObjectWithTag("Enemy");
        nav = GetComponent<NavMeshAgent>();
        findAlly();

        m_Collider = NearestAlly.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        heal();
        if (NearestAlly.tag == "dead")
        {
            findAlly();
            return;
        }
        else
        {
            nav.SetDestination(NearestAlly.transform.position);
        }
    }

    public void findAlly()
    {
        nearestDistance = 1000;
        Allies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Allies.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, Allies[i].transform.position);

            if(distance < nearestDistance)
            {
                
                NearestAlly = Allies[i];
                nearestDistance = distance;
            }
        }
        m_Collider = NearestAlly.GetComponent<Collider>();
    }
    public void heal()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            if (nearby.gameObject.tag == "Enemy")
            {
                if (cd == false)
                {
                    nearby.GetComponent<Enemy_health>().GainLife(healAmount);
                    StartCoroutine(Tik());
                }
            }
        }
    }
    IEnumerator Tik()
    {
        cd = true;
        yield return new WaitForSeconds(.3f);
        cd = false;
    }
}
