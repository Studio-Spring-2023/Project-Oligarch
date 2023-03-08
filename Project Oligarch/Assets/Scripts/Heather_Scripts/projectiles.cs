using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    public int dam;

    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float timeBetweenShots;

    public bool shooting;
    public bool canShoot;

    public Camera attackCam;
    public Transform attackPoint;
    public RaycastHit hit;
    public LayerMask isEnemy;
    public GameObject homingMissile;

    private void Awake ( )
    {
        canShoot = true;
    }

    private void Update ( )
    {
        ShootingInput ( );
    }

    private void ShootingInput ( )
    {
        if ( canShoot )
        {
            shooting = Input.GetKey ( KeyCode.Mouse0 );
        }
        else if ( Input.GetKeyDown ( KeyCode.Mouse1 ) )
        {
            GameObject.Instantiate ( homingMissile );
        }

        if(canShoot && shooting)
        {
            Bang ( );
        }
    }

    private void Bang ( )
    {
        canShoot= false;

        float x = Random.Range ( -spread , spread );
        float y = Random.Range ( -spread , spread );

        Vector3 dir = attackCam.transform.forward + new Vector3 ( x , y , 0 );

        if ( Physics.Raycast ( attackCam.transform.position , dir , out hit , range , isEnemy ) )
        {
            Debug.Log ( hit.collider.name );

            if ( hit.collider.CompareTag ( "Enemy" ) )
            {
                Debug.Log ( "Enemy took Damage" );
            }
        }

        Invoke ( "ResetShot" , timeBetweenShooting );

        Invoke ( "Bang" , timeBetweenShooting );
    }

    private void ResetShot ( )
    {
        canShoot = true;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.DrawLine(attackCam.transform.position,hit.point );
        Gizmos.color= Color.black;

        Gizmos.DrawSphere ( hit.point , .1f );
        Gizmos.color= Color.red;
    }
}
