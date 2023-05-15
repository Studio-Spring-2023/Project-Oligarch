using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( LineRenderer ) )]
public class projectiles : MonoBehaviour
{
    //These are the stats for the Gun
    private int BulletsToShoot;
    private int BulletsPerTap;
    private int BulletsLeft;
    private int BulletsShot;
    

    public static float Dam = 2f;   
    public static float TimeBetweenShooting = 0.5f;
    public float Spread;
    public float Range;
    private float reloadTime;
    public float TimeBetweenShots;
    public float bulletsDuration = 0.1f;
    private Vector3 dir;
    public static float LifeSteal = 0f;

    //These are the different bools for the Gun
    private bool Shooting;
    private bool CanShoot;
    private bool reloading;

    //These are different things that will be refenced in the code
    public Transform AttackPoint;
    public RaycastHit Hit;
    public LayerMask IsEnemy;
    public Transform Player;
    public PlayerCore player;

    LineRenderer bullets;

    public Animator playerRanged;


    private void Start ( )
    {
        bullets = GetComponent<LineRenderer> ( );
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCore> ( );
        //This sets up the weapon so that the bullets are set so there will be a burst of 3 and it is ready to shoot and sets the range for the gun
        BulletsToShoot = 1;
        BulletsLeft=BulletsToShoot;
        BulletsPerTap = BulletsToShoot;
        //Range = 20;
        CanShoot = true;
    }

    private void Update ( )
    {
        //AttackPoint.rotation = player.CameraTransform.rotation;
        ShootingInput ( );

        //fires gun visual even if you miss
        if(Input.GetKeyDown( KeyCode.Mouse0 ) )
        {

            //StartCoroutine ( ShotBullet ( ) );
        }
    }

    private void ShootingInput ( )
    {
            if (Shooting)
            {
                playerRanged.SetBool("Shooting", true);
            }
            else
            {
                playerRanged.SetBool("Shooting", false);
            }
        
        //This is to make sure that the left mouse button is the one that controls the gun
        if ( CanShoot )
        {
            
            Shooting = Input.GetKey ( KeyCode.Mouse0 );
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
        CanShoot = false;

        //This is the spread
        float x = Random.Range ( -Spread , Spread );
        float y = Random.Range ( -Spread , Spread );

        bullets.SetPosition ( 0 , AttackPoint.position );

        //This calculates the diration with the spread as a factor
        dir = (player.RotatedCrosshairPoint - player.CameraTransform.position).normalized;

        //This is the raycast for shooting
        if ( Physics.Raycast ( AttackPoint.transform.position , dir , out Hit , Mathf.Infinity , IsEnemy ) )
        {
            //Debug.Log(Hit.collider.gameObject.name);
            //Hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            bullets.SetPosition ( 1 , Hit.point );
            //Debug.Log(Hit.collider.gameObject.GetComponent<Enemy_health>());
            Hit.collider.gameObject.GetComponent<Enemy_health> ( ).LoseLife ( Dam );
            TakeHealthDamage.currentHealth += (int)(LifeSteal * Dam);
            StartCoroutine ( ShotBullet ( ) );
        }
        else
        {
            
            Vector3 temppos =(AttackPoint.position - (player.RotatedCrosshairPoint - player.CameraTransform.position).normalized * - Range);
            bullets.SetPosition(1, temppos);
            StartCoroutine(ShotBullet());            
        }

        //This tracks how many bullets have been shot and how many are left
        BulletsLeft--;
        BulletsShot--;

        //This Resets the CanShoot Bool
        Invoke ( "resetShot" , TimeBetweenShooting );


        //This keeps track of the bursts
        if ( BulletsShot > 0 && BulletsLeft > 0 )
            Invoke ( "bang" , TimeBetweenShots);
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

    public GameObject muzzleflash;
    public GameObject gunEnd;
    IEnumerator ShotBullet ( )
    {
        GameObject _exp = Instantiate(muzzleflash, gunEnd.transform.position, gunEnd.transform.rotation);
        Destroy(_exp, 1.5f);
        bullets.enabled = true;
        yield return new WaitForSeconds ( bulletsDuration );
        bullets.enabled= false;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(AttackPoint.transform.position, (player.RotatedCrosshairPoint - player.CameraTransform.position).normalized * Range); 

        Gizmos.color = Color.red;
        Gizmos.DrawSphere ( Hit.point , .1f );
    }
}
