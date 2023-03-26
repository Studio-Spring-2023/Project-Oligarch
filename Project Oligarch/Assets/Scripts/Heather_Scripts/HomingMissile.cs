using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{

    //These are different things that will be referenced in the code
    public GameObject target;
    private Rigidbody rb;
    public projectiles projectiles;

    //These are missile stats
    public float force;
    public float rotationForce;


    private void Awake ( )
    {
        //This makes sure that there is a rigidbody and that the missile is a child of the weapon that fired it and that it has the necessary script to find it's target
        rb = this.GetComponent<Rigidbody>();
        transform.SetParent ( GameObject.FindGameObjectWithTag ( "Pistol" ).transform );
        projectiles=GetComponentInParent<projectiles> ();
    }

    private void Update ( )
    {
        //This locates it's target
        target = projectiles.Hit.collider.gameObject;
    }

    private void FixedUpdate ( )
    {
        //This sends it to it's target
            Vector3 dir = ( target.transform.position - rb.position ).normalized;
            Vector3 rotationAmount = Vector3.Cross ( transform.forward , dir );
            rb.angularVelocity = rotationAmount * rotationForce;
            rb.velocity = transform.forward * force;
       
    }

    private void OnCollisionEnter ( Collision boom )
    {
        //This damages it's target before destroying itself
        boom.collider.GetComponent<Enemy_health> ( ).LoseLife ( 5 );
        Destroy ( gameObject );
    }
}
