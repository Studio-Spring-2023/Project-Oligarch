using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float lingerTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lingerTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
