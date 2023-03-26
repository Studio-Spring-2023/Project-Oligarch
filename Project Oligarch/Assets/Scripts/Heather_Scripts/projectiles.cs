using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    public int Dam;
    public int BulletsToShoot;
    public int BulletsPerTap;
    private int BulletsLeft;
    private int BulletsShot;

    public float TimeBetweenShooting;
    public float Spread;
    public float Range;
    private float reloadTime;
    public float TimeBetweenShots;

    public bool Shooting;
    public bool CanShoot;
    private bool reloading;

    public Transform AttackPoint;
    public RaycastHit Hit;
    public LayerMask IsEnemy;
    public GameObject HomingMissile;

    private void Awake ( )
    {
        BulletsLeft=BulletsToShoot;
        BulletsPerTap = BulletsToShoot;
        CanShoot = true;
    }

    private void Update ( )
    {
        ShootingInput ( );
    }

    private void ShootingInput ( )
    {
        if ( CanShoot )
        {
            Shooting = Input.GetKey ( KeyCode.Mouse0 );
        }
       
        if ( Input.GetKeyDown ( KeyCode.Mouse1 ) )
        {
            GameObject.Instantiate ( HomingMissile );
        }

        if ( BulletsLeft < BulletsToShoot && !reloading )
            reload ( );


        if ( CanShoot && Shooting && !reloading && BulletsLeft > 0 )
        {
            BulletsShot = BulletsPerTap;
            bang ( );
        }
    }

    private void bang ( )
    {
        CanShoot= false;

        float x = Random.Range ( -Spread , Spread );
        float y = Random.Range ( -Spread , Spread );

        Vector3 dir = AttackPoint.transform.forward + new Vector3 ( x , y , 0 );

        if ( Physics.Raycast ( AttackPoint.transform.position , dir , out Hit , Range , IsEnemy ) )
        {
            Debug.Log ( Hit.collider.name );

            if ( Hit.collider.CompareTag ( "Enemy" ) )
            {
                Hit.collider.GetComponent<Enemy_health> ( ).LoseLife ( 1 );
            }
        }
        BulletsLeft--;
        BulletsShot--;

        Invoke ( "resetShot" , TimeBetweenShooting );

        if ( BulletsShot > 0 && BulletsLeft > 0 )
            Invoke ( "bang" , TimeBetweenShots );
    }

    private void resetShot ( )
    {
        CanShoot = true;
    }

    private void reload()
    {
        reloading = true;
        Invoke ( "reloadFinished" , reloadTime );
    }

    private void reloadFinished ( )
    {
        BulletsLeft = BulletsToShoot;
        reloading = false;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(AttackPoint.transform.position, AttackPoint.transform.forward );

        Gizmos.color = Color.red;
        Gizmos.DrawSphere ( Hit.point , .1f );        
    }
}
