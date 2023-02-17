using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_New_Input : MonoBehaviour
{
    PlayerInput playerInput;

    [HideInInspector]
    public Vector2 input_Move;
    [HideInInspector]
    public Vector2 input_Look;

    public Transform camTarget;

    private void Awake ( )
    {
        playerInput = new PlayerInput ( );

        playerInput.Movement.Move.performed += x => input_Move = x.ReadValue<Vector2> ( );
        playerInput.Movement.View.performed += x => input_Look = x.ReadValue<Vector2> ( );
        playerInput.Actions.Jump.performed += x => Jump ( );

        playerInput.Enable ( );
    }

    private void Jump ( )
    {
        Debug.Log ( "I'm Jumping" );
    }

}
