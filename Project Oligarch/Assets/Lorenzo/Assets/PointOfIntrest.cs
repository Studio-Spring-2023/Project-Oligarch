using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfIntrest : MonoBehaviour
{
    public float Range;
    private Transform Playertrans;
    void Start()
    {
        Playertrans = GameObject.FindWithTag("Player").transform;
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Range);  
    }
}
