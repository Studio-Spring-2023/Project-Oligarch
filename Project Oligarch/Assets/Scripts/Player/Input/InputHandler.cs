using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
public class InputHandler : MonoBehaviour
{
    //Singleton for InputHandler
    public static InputHandler PlayerInput { get; private set; }

    //Input System Maps & Actions
	private PlayerControls playerInputActions;
	private PlayerControls.CameraActions CameraControls;
	private PlayerControls.MovementActions MovementControls;
	private PlayerControls.InteractActions InteractControls;
	private PlayerControls.AbilitiesActions AbilityControls;
	private PlayerControls.UIActions UIControls;

	//Mouse Global Variables
	public static Vector2 MouseDelta { get; private set; }
	public static Vector2 MouseScreenPos { get; private set; }
	public static Vector2 MouseScroll { get; private set; }
	public static Vector2 MovementInput { get; private set; }

    //Ability Global Variables
    public static AbilityInput LastAbilityInput { get; private set; }

    //Events
	public static event Action OnAbilityInput;

    public void Awake()
    {
        PlayerInput = this;

		playerInputActions = new PlayerControls();

        CameraControls = playerInputActions.Camera;
        MovementControls = playerInputActions.Movement;
        InteractControls = playerInputActions.Interact;
        AbilityControls = playerInputActions.Abilities;
        UIControls = playerInputActions.UI;
    }

    public void OnEnable()
    {
        CameraControls.Enable();
        MovementControls.Enable();
        InteractControls.Enable();
        AbilityControls.Enable();
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
    private void CacheMovementInput(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }

    private void ClearMovementInput(InputAction.CallbackContext ctx)
    {
        MovementInput = Vector2.zero;
    }

    private void CacheJumpInput(InputAction.CallbackContext ctx)
    {
        
    }
    #endregion

    #region Interact Controls
    private void ReceivedInteractInput(InputAction.CallbackContext ctx)
    {
        
    }
    #endregion

    #region Abilities Controls
    private void PrimaryAbilityInput(InputAction.CallbackContext ctx)
    {
        LastAbilityInput = AbilityInput.Primary;
        OnAbilityInput?.Invoke();
	}

    private void SecondaryAbilityInput(InputAction.CallbackContext ctx)
    {
		LastAbilityInput = AbilityInput.Secondary;
		OnAbilityInput?.Invoke();
	}

	private void SpecialAbilityInput(InputAction.CallbackContext ctx)
    {
		LastAbilityInput = AbilityInput.Special;
		OnAbilityInput?.Invoke();
	}
	private void UltimateAbilityInput(InputAction.CallbackContext ctx)
    {
		LastAbilityInput = AbilityInput.Ultimate;
		OnAbilityInput?.Invoke();
	}
	#endregion

	#region UI Controls
	private void UIPauseInput(InputAction.CallbackContext ctx)
    {

    }

    private void UISelectInput(InputAction.CallbackContext ctx)
    {

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
}