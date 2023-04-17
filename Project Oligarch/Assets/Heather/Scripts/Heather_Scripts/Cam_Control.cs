using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class Cam_Control : MonoBehaviour
{
    public Player_Movement_New_Input playerMove;
    public CamSettings settings;

    private Vector3 targetRotate;
    private Vector3 yGimbalRotate;

    public GameObject yGimbal;

    private void Update ( )
    {
        FollowPlayer ( );
        CamRotate ( );
    }

    private void CamRotate ( )
    {
        var viewInput = playerMove.input_Look;

        targetRotate.y += ( settings.invertX ? -( viewInput.x * settings.sensX ) : ( viewInput.x * settings.sensX ) ) * Time.deltaTime;
        transform.rotation = Quaternion.Euler ( targetRotate );

       yGimbalRotate.x += ( settings.invertY ? ( viewInput.y * settings.sensY ) : -( viewInput.y * settings.sensY ) ) * Time.deltaTime;
        yGimbalRotate.x = Mathf.Clamp ( yGimbalRotate.x , settings.yClampMin , settings.yClampMax );      
        yGimbal.transform.localRotation= Quaternion.Euler ( yGimbalRotate );

        if ( playerMove.isLookTarget )
        {
            var currentRotate = playerMove.transform.rotation;
            var newRotate = currentRotate.eulerAngles;

            newRotate.y=targetRotate.y;
            currentRotate = Quaternion.Lerp ( currentRotate , Quaternion.Euler ( newRotate ) , settings.playerRotateSmooth );

            playerMove.transform.rotation=currentRotate;
        }
    }

    private void FollowPlayer ( )
    {
        transform.position = playerMove.camTarget.position;
    } 
}
