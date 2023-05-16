using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    
    public float neededSpeed;
    public GameObject explosion;
    public float force, radius, enemydamage;
    public int playerdamage;
    public float lingerTime = 3;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Velocity:" + this.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
    }

    void OnTriggerEnter()
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude > neededSpeed)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Vector3 sourcePosition = transform.position;
        SoundManager.instance.PlaySound(5, sourcePosition);

        GameObject _exp = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(_exp, lingerTime);
        knockBack();
        Destroy(gameObject);

    }

    public void knockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rigg = nearby.GetComponent<Rigidbody>();
            if(rigg != null)
            {
                rigg.AddExplosionForce(force, transform.position, radius);
            }
            if (nearby.gameObject.tag == "Enemy")
            {
                nearby.GetComponent<Enemy_health>().LoseLife(enemydamage);
            }
            if (nearby.gameObject.tag == "Player")
            {
                PlayerCore.Damaged(playerdamage);
            }


        }

    }
}
