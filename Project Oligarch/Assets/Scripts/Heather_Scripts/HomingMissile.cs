using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;

    public float force;
    public float rotationForce;


    private void Awake ( )
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update ( )
    {
        target = 
    }

    private void FixedUpdate ( )
    {
            Vector3 dir = ( target.position - rb.position ).normalized;
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
