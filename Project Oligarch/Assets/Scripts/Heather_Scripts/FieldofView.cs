using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    //These are for controlling the field of view
    [Range ( 0 , 5 )]
    public float viewRadius;
    [Range ( 0 , 100 )]
    public float viewAngle;

    //These are refrences for the missile
    public LayerMask Enemy;
    public GameObject HomingMissile;
    public HomingMissile missile;

    private void Update ( )
    {
        MissileTargeting ( );
    }

    public Vector3 DirFromAngle ( float angleInDegrees , bool angleIsGlobal )
    {
        //This makes it so the field of view can turn with the player
        if ( !angleIsGlobal )
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        //This is the math for the field of view
        return new Vector3 ( Mathf.Sin ( angleInDegrees * Mathf.Deg2Rad ) , 0 , Mathf.Cos ( angleInDegrees * Mathf.Deg2Rad ) );
    }

    //This is the math for the missile to use the field of view to aquire a target after spawning
    public void MissileTargeting ( )
    {
        Collider [ ] targetsInViewRadius = Physics.OverlapSphere ( transform.position , viewRadius , Enemy );

        for ( int i = 0 ; i < targetsInViewRadius.Length ; i++ )
        {
            Transform target = targetsInViewRadius [ i ].transform;
            Vector3 dirToTarget = ( target.position - transform.position ).normalized;

            if ( Vector3.Angle ( transform.forward , dirToTarget ) < viewAngle / 2 )
            {
                float disToTarget = Vector3.Distance ( transform.position , target.position );

                if ( disToTarget < 5 && Input.GetKeyDown ( KeyCode.Mouse1 ) )
                {
                    GameObject.Instantiate ( HomingMissile );
                    HomingMissile.transform.position = transform.position;
                    missile.target = target.transform;
                }
            }
        }
    }
}
