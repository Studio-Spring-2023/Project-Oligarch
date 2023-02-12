using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerInputActions;

    private PlayerControls.CameraActions CameraControls;
    private PlayerControls.MovementActions MovementControls;
    private PlayerControls.InteractActions InteractControls;
    private PlayerControls.AbilitiesActions AbilityControls;
    private PlayerControls.UIActions UIControls;

    public static Vector2 MouseDelta { get; private set; }
    public static Vector2 MouseScreenPos { get; private set; }
    public static Vector2 MouseScroll { get; private set; }
    public static Vector2 MovementInput { get; private set; }

    //public static event Action<InteractInput> OnInteractInput;
    //public static event Action OnMenuInput;

    public void Awake()
    {
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

        AbilityControls.Primary.performed += AbilityPrimaryInput;
        AbilityControls.Secondary.performed += AbilitySecondaryInput;
        AbilityControls.Special.performed += AbilitySpecialInput;
        AbilityControls.Ultimate.performed += AbilityUltimateInput;

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
        MovementInput = ctx.ReadValue<Vector2>();
    }

    private void CacheMouseScroll(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
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
        //OnInteractInput?.Invoke(InputType);
    }
    #endregion

    #region Abilities Controls
    private void AbilityPrimaryInput(InputAction.CallbackContext ctx)
    {

    }

    private void AbilitySecondaryInput(InputAction.CallbackContext ctx)
    {

    }

    private void AbilitySpecialInput(InputAction.CallbackContext ctx)
    {

    }
    private void AbilityUltimateInput(InputAction.CallbackContext ctx)
    {

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

        AbilityControls.Primary.performed -= AbilityPrimaryInput;
        AbilityControls.Secondary.performed -= AbilitySecondaryInput;
        AbilityControls.Special.performed -= AbilitySpecialInput;
        AbilityControls.Ultimate.performed -= AbilityUltimateInput;

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