using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    public int dam;
    public int magSize;
    public int bulletsPerTap;
    public int bulletsLeft;
    public int bulletsShot;

    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;

    public bool canHold;
    public bool shooting;
    public bool canShoot;
    public bool reloading;

    public Camera attackCam;
    public Transform attackPoint;
    public RaycastHit hit;
    public LayerMask isEnemy;

    private void Awake ( )
    {
        bulletsLeft = magSize;
        canShoot = true;
    }

    private void Update ( )
    {
        ShootingInput ( );
    }

    private void ShootingInput ( )
    {
        if ( canHold )
        {
            shooting = Input.GetButton ( "Fire1" );
        }
        else
        {
            shooting = Input.GetButtonDown ( "Fire1" );
        }


        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading )
        {
            Reload ( );
        }

        if(canShoot && shooting && !reloading && bulletsLeft > 0 )
        {
            bulletsShot = bulletsPerTap;
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

        bulletsLeft--;
        bulletsShot--;

        Invoke ( "ResetShot" , timeBetweenShooting );
        if(bulletsShot>0 && bulletsLeft > 0 )
        {
            Invoke ( "Bang" , timeBetweenShooting );
        }
    }

    private void ResetShot ( )
    {
        canShoot = true;
    }

    private void Reload ( )
    {
        reloading= true;
        Invoke ( "ReloadFinished" , reloadTime );
    }

    private void ReloadFinished ( )
    {
        bulletsLeft = magSize;
        reloading = false;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.DrawLine(attackCam.transform.position,hit.point );
        Gizmos.color= Color.black;

        Gizmos.DrawSphere ( hit.point , .1f );
        Gizmos.color= Color.red;
    }
}
