using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [Header ( "Settings" )]
    public float lifeTime = 1;

    private void Awake ( )
    {
        Destroy ( gameObject , lifeTime );
    }
}
