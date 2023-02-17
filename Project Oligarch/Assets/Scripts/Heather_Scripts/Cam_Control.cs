using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class Cam_Control : MonoBehaviour
{
    public Player_Movement_New_Input playerMove;
    public PlayerSettings settings;

    private Vector3 targetRotate;

    private void Update ( )
    {
        FollowPlayer ( );
        CamRotate ( );
    }

    private void CamRotate ( )
    {
        var viewInput = playerMove.input_Look;

        targetRotate.x += ( settings.invertY ? ( viewInput.y * settings.sensY ) : -( viewInput.y * settings.sensY ) ) * Time.deltaTime;
        targetRotate.y += ( settings.invertX ? -( viewInput.x * settings.sensX ) : ( viewInput.x * settings.sensX ) ) * Time.deltaTime;
        targetRotate.x = Mathf.Clamp ( targetRotate.x , settings.yClampMin , settings.yClampMax );

        transform.rotation = Quaternion.Euler ( targetRotate );

        if ( playerMove.isLookTarget )
        {
            var currentRotate = playerMove.transform.rotation.eulerAngles;
            currentRotate.y = targetRotate.y;

            playerMove.transform.rotation=Quaternion.Euler ( currentRotate );
        }
    }

    private void FollowPlayer ( )
    {
        transform.position = playerMove.camTarget.position;
    }
}
