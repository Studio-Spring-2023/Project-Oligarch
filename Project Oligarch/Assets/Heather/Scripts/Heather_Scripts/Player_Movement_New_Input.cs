using UnityEngine;
using static Models;

public class Player_Movement_New_Input : MonoBehaviour
{
    //PlayerInput playerInput;
    //CharacterController playerControl;
    //public Cam_Control camControl;
    //public Transform camTarget;
    //public PlayerControl settings;

    //[HideInInspector]
    //public Vector2 input_Move;
    //[HideInInspector]
    //public Vector2 input_Look;

    //Vector3 playerWalk;

    //public bool isLookTarget;

    //private void Awake ( )
    //{
    //    playerInput = new PlayerInput ( );
    //    playerControl = GetComponent<CharacterController> ( );

    //    playerInput.Movement.Move.performed += x => input_Move = x.ReadValue<Vector2> ( );
    //    playerInput.Movement.View.performed += x => input_Look = x.ReadValue<Vector2> ( );
    //    playerInput.Actions.Jump.performed += x => Jump ( );

    //    playerInput.Enable ( );
    //}

    //private void Update ( )
    //{
    //    Move ( );
    //}

    //private void Move ( )
    //{
    //    playerWalk = camControl.transform.forward * ( settings.forwardSpeed * input_Move.y ) * Time.deltaTime;
    //    playerWalk += camControl.transform.right * ( settings.forwardSpeed * input_Move.x ) * Time.deltaTime;

    //    if ( !isLookTarget )
    //    {
    //        var originalRotate=transform.rotation;
    //        transform.LookAt ( playerWalk + transform.position , Vector3.up );
    //        var newRotate=transform.rotation;

    //        transform.rotation = Quaternion.Lerp ( originalRotate , newRotate , settings.playerSmooth );
    //    }

    //    playerControl.Move ( playerWalk );
    //}

    //private void Jump ( )
    //{
    //    Debug.Log ( "I'm Jumping" );
    //}

}
