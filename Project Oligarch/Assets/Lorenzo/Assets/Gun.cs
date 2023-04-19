using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public PlayerCore player;
    public Transform FirePoint;
    Vector3 dirFromCameraToCrosshair;
    private 
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCore>();
        dirFromCameraToCrosshair = player.RotatedCrosshairPoint - player.CameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShootBullet()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(FirePoint.position , (player.RotatedCrosshairPoint - player.CameraTransform.position).normalized * 10f);
    }
}
