using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.SetSoundSpeed(8, 0.5f);
        Vector3 sourcePosition = gameObject.transform.position;
        SoundManager.instance.PlaySound(8, sourcePosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
