using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{

    //These are different things that will be referenced in the code
    public Transform target;
    private Rigidbody rb;
    public FieldofView FOV;


    //These are missile stats
    public float Dam;
    public float damMod;
    public float atkSpeedMod;
    public float force;
    public float rotationForce;
    public bool missileWait = true;


    private void Awake ( )
    {
        StartCoroutine(waitToHit());
        //This makes sure that there is a rigidbody and that the missile is a child of the weapon that fired it and that it has the necessary script to find it's target
        rb = this.GetComponent<Rigidbody> ( );
        FOV=GetComponentInParent<FieldofView> ();
    }

    private void FixedUpdate ( )
    {
        //This sends it to it's target
            Vector3 dir = ( target.transform.position - rb.position ).normalized;
            Vector3 rotationAmount = Vector3.Cross ( transform.up , dir );
            rb.angularVelocity = rotationAmount * rotationForce;
            rb.velocity = transform.up * force * ( 1 + atkSpeedMod );
    }

    private void OnCollisionEnter ( Collision boom )
    {
        //This damages it's target before destroying itself
        if (missileWait == false)
        {
            if (boom.collider.CompareTag("Enemy"))
            {
                boom.collider.GetComponent<Enemy_health>().LoseLife(Dam * (1 + damMod));
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator waitToHit()
    {
        yield return new WaitForSeconds(.2f);
        missileWait = false;
    }
}
