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
       
        if ( Input.GetKeyDown ( KeyCode.Mouse1 ) )
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

        Vector3 dir = attackPoint.transform.forward + new Vector3 ( x , y , 0 );

        if ( Physics.Raycast ( attackPoint.transform.position , dir , out hit , range , isEnemy ) )
        {
            Debug.Log ( hit.collider.name );

            if ( hit.collider.CompareTag ( "Enemy" ) )
            {
                FindObjectOfType<Enemy_health> ( ).LoseLife ( 1 );
            }
        }

        Invoke ( "ResetShot" , timeBetweenShooting );
    }

    private void ResetShot ( )
    {
        canShoot = true;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.DrawLine(attackPoint.transform.position,hit.point );
        Gizmos.color= Color.black;

        Gizmos.DrawSphere ( hit.point , .1f );
        Gizmos.color= Color.red;
    }
}
