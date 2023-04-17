using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform point;
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        point = cam;   
    }
}
