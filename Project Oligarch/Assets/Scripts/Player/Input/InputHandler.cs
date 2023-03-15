using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public enum AbilityInput
{
    Primary,
    Secondary,
    Special,
    Ultimate,
    None
}

/// <summary>
/// Handles all input from the Player. 
/// Contains static events to immediately trigger input related behavior.
/// </summary>
public class InputHandler
{
    #region Input System Map & Action Variables
	private PlayerControls playerInputActions;
	private PlayerControls.CameraActions CameraControls;
	private PlayerControls.MovementActions MovementControls;
	private PlayerControls.InteractActions InteractControls;
	private PlayerControls.AbilitiesActions AbilityControls;
	private PlayerControls.UIActions UIControls;
	#endregion

	#region Mouse Input Variables
	public static Vector2 MouseDelta { get; private set; }
	public static Vector2 MouseScreenPos { get; private set; }
	public static Vector2 MouseScroll { get; private set; }
	#endregion

	#region Movement Input Variables
	public static Vector3 MovementInput { get; private set; }
	#endregion

	//Ability Global Variables
	public static AbilityInput LastAbilityInput { get; private set; }

    #region Events
    public static Action OnJumpInput;
	public static Action OnInteractInput;
	#endregion

	#region InputHandler Constructor
	public InputHandler()
    {
		playerInputActions = new PlayerControls();

		CameraControls = playerInputActions.Camera;
		MovementControls = playerInputActions.Movement;
		InteractControls = playerInputActions.Interact;
		AbilityControls = playerInputActions.Abilities;
		UIControls = playerInputActions.UI;
	}
	#endregion

	#region OnEnable
	public void OnEnable()
    {
        CameraControls.Enable();
        MovementControls.Enable();
        InteractControls.Enable();
        UIControls.Enable();

        CameraControls.MouseDelta.performed += CacheMouseDelta;
        CameraControls.MousePos.performed += CacheMouseScreenPos;
        CameraControls.MouseScroll.performed += CacheMouseScroll;

        MovementControls.Walk.performed += CacheMovementInput;
        MovementControls.Walk.canceled += ClearMovementInput;
        MovementControls.Jump.performed += CacheJumpInput;

        InteractControls.Interact.performed += ReceivedInteractInput;

        AbilityControls.Primary.performed += PrimaryAbilityInput;
        AbilityControls.Secondary.performed += SecondaryAbilityInput;
        AbilityControls.Special.performed += SpecialAbilityInput;
        AbilityControls.Ultimate.performed += UltimateAbilityInput;

        UIControls.Pause.performed += UIPauseInput;
        UIControls.Select.performed += UISelectInput;
    }
	#endregion

	#region Mouse Controls
	private void CacheMouseDelta(InputAction.CallbackContext ctx)
    {
        MouseDelta = ctx.ReadValue<Vector2>();
    }

    private void CacheMouseScreenPos(InputAction.CallbackContext ctx)
    {
        MouseScreenPos = ctx.ReadValue<Vector2>();
    }

    private void CacheMouseScroll(InputAction.CallbackContext ctx)
    {
        MouseScroll = ctx.ReadValue<Vector2>();
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    #region Movement Controls
    void CacheMovementInput(InputAction.CallbackContext ctx)
    {
		MovementInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
    }

    private void ClearMovementInput(InputAction.CallbackContext ctx)
    {
        MovementInput = Vector2.zero;
    }

    private void CacheJumpInput(InputAction.CallbackContext ctx)
    {
        OnJumpInput?.Invoke();
	}
    #endregion

    #region Interact Controls
    private void ReceivedInteractInput(InputAction.CallbackContext ctx)
    {
		OnInteractInput?.Invoke();
	}
	#endregion

	#region Abilities Controls
	public void EnableAbilityInputs()
	{
		AbilityControls.Enable();
	}

	public void DisableAbilityInputs()
    {
		AbilityControls.Disable();
	}

    private void PrimaryAbilityInput(InputAction.CallbackContext ctx)
    {
		PlayerCore.AssignedLoadout.Primary();
	}

    private void SecondaryAbilityInput(InputAction.CallbackContext ctx)
    {
		PlayerCore.AssignedLoadout.Secondary();
	}

	private void SpecialAbilityInput(InputAction.CallbackContext ctx)
    {
		PlayerCore.AssignedLoadout.Special();
	}
	private void UltimateAbilityInput(InputAction.CallbackContext ctx)
    {
        PlayerCore.AssignedLoadout.Ultimate();
	}
	#endregion

	#region UI Controls
	private void UIPauseInput(InputAction.CallbackContext ctx)
    {

    }

    private void UISelectInput(InputAction.CallbackContext ctx)
    {
        Debug.Log("UI Select Input");
    }

    public static void EnterUIMode()
    {
        MouseDelta = Vector2.zero;
        MovementInput = Vector2.zero;

        Cursor.lockState = CursorLockMode.None;
    }

    public static void ExitUIMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
	#endregion

	#region OnDisable
	public void OnDisable()
    {
        UIControls.Pause.performed -= UIPauseInput;
        UIControls.Select.performed -= UISelectInput;

        AbilityControls.Primary.performed -= PrimaryAbilityInput;
        AbilityControls.Secondary.performed -= SecondaryAbilityInput;
        AbilityControls.Special.performed -= SpecialAbilityInput;
        AbilityControls.Ultimate.performed -= UltimateAbilityInput;

        InteractControls.Interact.performed -= ReceivedInteractInput;

        MovementControls.Walk.performed -= CacheMovementInput;
        MovementControls.Walk.canceled -= ClearMovementInput;
        MovementControls.Jump.performed -= CacheJumpInput;

        CameraControls.MouseDelta.performed -= CacheMouseDelta;
        CameraControls.MousePos.performed -= CacheMouseScreenPos;
        CameraControls.MouseScroll.performed -= CacheMouseScroll;

        CameraControls.Disable();
        MovementControls.Disable();
        InteractControls.Disable();
        AbilityControls.Disable();
        UIControls.Disable();
    }
	#endregion
}