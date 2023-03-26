using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rb;
    public projectiles projectiles;

    public float force;
    public float rotationForce;


    private void Awake ( )
    {
        rb = this.GetComponent<Rigidbody>();
        transform.SetParent ( GameObject.FindGameObjectWithTag ( "Pistol" ).transform );
        projectiles=GetComponentInParent<projectiles> ();
    }

    private void Update ( )
    {
        target = projectiles.Hit.collider.gameObject;
    }

    private void FixedUpdate ( )
    {
            Vector3 dir = ( target.transform.position - rb.position ).normalized;
            Vector3 rotationAmount = Vector3.Cross ( transform.forward , dir );
            rb.angularVelocity = rotationAmount * rotationForce;
            rb.velocity = transform.forward * force;
       
    }

    private void OnCollisionEnter ( Collision boom )
    {
        boom.collider.GetComponent<Enemy_health> ( ).LoseLife ( 5 );
        Destroy ( gameObject );
    }
}
