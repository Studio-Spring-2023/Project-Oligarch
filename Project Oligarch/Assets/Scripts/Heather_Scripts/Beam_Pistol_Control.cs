using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam_Pistol_Control : MonoBehaviour
{
    public int dam;
    public int bulletsPerTap;
    public int bulletsShot;
    public int magSize;
    public int bulletsLeft;

    public float cooldown;
    public float range;
    public float timeForShots;

    public bool canHold;
    public bool shooting;
    public bool reloading;
    public bool canShoot;

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
            shooting = Input.GetKey ( KeyCode.Mouse0);
            Debug.Log ( "Shooting" );
        }
        else
        {
            shooting = Input.GetKeyDown ( KeyCode.Mouse0);
            Debug.Log ( "Shot" );
        }

        if ( Input.GetKeyDown ( KeyCode.R ) && bulletsLeft < magSize && !reloading )
            Reload ( );


        if ( canShoot && shooting && !reloading && bulletsLeft > 0 )
        {
            bulletsShot = bulletsPerTap;
            Bang ( );
        }

    }

    private void Bang ( )
    {
        canShoot= false;

        Vector3 start=attackPoint.transform.position;
        Vector3 dir= attackPoint.transform.forward;
        Ray beamShot = new Ray ( start , start + dir );

        if(Physics.Raycast(beamShot, out hit, range, isEnemy) )
        {
            Debug.Log ( hit.collider.name );

            if ( hit.collider.CompareTag ( "Enemy" ) )
                Debug.Log ( "Enemy took damage" );
        }

        bulletsLeft--;
        bulletsShot--;

        Invoke ( "ResetShot" , cooldown );

        if ( bulletsShot > 0 && bulletsLeft > 0 )
        {
            Invoke ( "Bang" , timeForShots );
        }
    }

    private void ResetShot ( )
    {
        canShoot = true;
    }

    private void Reload ( )
    {
        reloading= true;
        Invoke ( "ReloadFinished" , cooldown );
    }

    private void ReloadFinished ( )
    {
        bulletsLeft = magSize;
        reloading = false;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.DrawRay ( attackPoint.position , attackPoint.forward );
        Gizmos.color = Color.black;

        Gizmos.DrawSphere ( hit.point , .1f );
        Gizmos.color = Color.red;
    }

}
