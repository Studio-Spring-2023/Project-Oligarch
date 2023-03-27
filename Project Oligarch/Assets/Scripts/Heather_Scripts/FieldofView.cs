using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,100)]
    public float viewAngle;

    public LayerMask Enemy;

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal )
    {
        if ( !angleIsGlobal )
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3 ( Mathf.Sin ( angleInDegrees * Mathf.Deg2Rad ) ,0 , Mathf.Cos ( angleInDegrees * Mathf.Deg2Rad ) );
    }

    public void MissileTargeting ( )
    {
        Collider [ ] targetsInViewRadius = Physics.OverlapSphere ( transform.position , viewRadius , Enemy );

        for(int i = 0; i<targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius [ i ].transform;
            Vector3 dirToTarget=(target.position - transform.position ).normalized;

            if ( Vector3.Angle ( transform.forward , dirToTarget ) < viewAngle / 2 )
            {
                float disToTarget = Vector3.Distance ( transform.position , target.position );
            }
        }
    }
}
