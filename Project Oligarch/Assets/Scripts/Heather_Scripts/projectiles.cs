using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    //These are the stats for the Gun
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

    //These are the different bools for the Gun
    public bool Shooting;
    public bool CanShoot;
    private bool reloading;


    //These are different things that will be refenced in the code
    public Transform AttackPoint;
    public RaycastHit Hit;
    public LayerMask IsEnemy;
    public GameObject HomingMissile;

    private void Awake ( )
    {
        //This sets up the weapon so that the bullets are set so there will be a burst of 3 and it is ready to shoot
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

        //This is to make sure that the left mouse button is the one that controls the gun
        if ( CanShoot )
        {
            Shooting = Input.GetKey ( KeyCode.Mouse0 );
        }
       
        //This is to launch a missile and that the right mouse button is the one controling it
        if ( Input.GetKeyDown ( KeyCode.Mouse1 ) )
        {
            GameObject.Instantiate ( HomingMissile );
        }

        //This is to autoreload so that we can have a burst of 3 bullets
        if ( BulletsLeft < BulletsToShoot && !reloading )
            reload ( );

        //This is where the shooting happens
        if ( CanShoot && Shooting && !reloading && BulletsLeft > 0 )
        {
            BulletsShot = BulletsPerTap;
            bang ( );
        }
    }

    private void bang ( )
    {
        CanShoot= false;

        //This is the spread
        float x = Random.Range ( -Spread , Spread );
        float y = Random.Range ( -Spread , Spread );

        //This calculates the diration with the spread as a factor
        Vector3 dir = AttackPoint.transform.forward + new Vector3 ( x , y , 0 );

        //This is the raycast for shooting
        if ( Physics.Raycast ( AttackPoint.transform.position , dir , out Hit , Range , IsEnemy ) )
        {
            Debug.Log ( Hit.collider.name );

            if ( Hit.collider.CompareTag ( "Enemy" ) )
            {
                Hit.collider.GetComponent<Enemy_health> ( ).LoseLife ( 1 );
            }
        }

        //This tracks how many bullets have been shot and how many are left
        BulletsLeft--;
        BulletsShot--;

        //This Resets the CanShoot Bool
        Invoke ( "resetShot" , TimeBetweenShooting );


        //This keeps track of the bursts
        if ( BulletsShot > 0 && BulletsLeft > 0 )
            Invoke ( "bang" , TimeBetweenShots );
    }

    private void resetShot ( )
    {
        CanShoot = true;
    }


    //This is the autoreload function and makes it so that the bursts happen without the player having to reload
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
