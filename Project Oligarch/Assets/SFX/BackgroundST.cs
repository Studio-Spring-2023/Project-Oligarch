using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 sourcePosition = gameObject.transform.position;
        SoundManager.instance.PlaySound(9, sourcePosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
