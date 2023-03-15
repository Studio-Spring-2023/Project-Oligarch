using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    public int Dam;

    public float TimeBetweenShooting;
    public float Spread;
    public float Range;
    public float TimeBetweenShots;

    public bool Shooting;
    public bool CanShoot;

    public Transform AttackPoint;
    public RaycastHit Hit;
    public LayerMask IsEnemy;
    public GameObject HomingMissile;

    private void Awake ( )
    {
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

        if(CanShoot && Shooting)
        {
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
                FindObjectOfType<Enemy_health> ( ).LoseLife ( 1 );
            }
        }

        Invoke ( "resetShot" , TimeBetweenShooting );
    }

    private void resetShot ( )
    {
        CanShoot = true;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.DrawLine(AttackPoint.transform.position,Hit.point );
        Gizmos.color= Color.black;

        Gizmos.DrawSphere ( Hit.point , .1f );
        Gizmos.color= Color.red;
    }
}
