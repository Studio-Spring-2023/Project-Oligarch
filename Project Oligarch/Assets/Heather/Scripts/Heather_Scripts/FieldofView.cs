using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    //These are for controlling the field of view
    [Range ( 0 , 100 )]
    public float viewRadius;
    [Range ( 0 , 180 )]
    public float viewAngle;

    //These are refrences for the missile
    public LayerMask Enemy;
    public GameObject HomingMissile;
    private HomingMissile missile;
    public GameObject missilePoint;
    public float CoolDown;
    private float startCD;
    private bool fired;
    public float CDRpercent;
    public float CDRflat;
    private float CDR;

    private void Start()
    {
        startCD = CoolDown;
    }

    private void Update ( )
    {
        MissileTargeting ( );
        if(fired)
        {
            CoolDown -= Time.deltaTime;
        }
        if(CoolDown <= 0)
        {
            fired = false;
            CoolDown = startCD;
        }
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

                if (disToTarget < viewRadius && Input.GetKeyDown(KeyCode.Mouse1) && !fired)
                {
                    missile=GameObject.Instantiate ( HomingMissile, missilePoint.transform.position, Quaternion.identity ).GetComponent<HomingMissile>();
                    
                    missile.target = target.transform;

                    StartCoroutine(CD());
                }
            }
        }
    }

    IEnumerator CD()
    {
        fired = true;
        CDR += CDRflat;
        CDR += CoolDown * CDRpercent;
        yield return new WaitForSeconds(CoolDown - CDR);
        fired = false;
        yield return null;
    }
}
