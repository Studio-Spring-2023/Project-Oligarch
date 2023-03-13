using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCore : Core
{
	public static Loadout AssignedLoadout { get; private set; }

	[Header("Movement Variables")]
    public Rigidbody PlayerRB;
	private Vector3 Velocity;
    private Vector3 Forward;
	[Range(1f, 5f)]
	public float JumpForce;
	[Range(-0.5f, -5f)]
	public float GravityForce;
	public float GroundCheckDistance;
	private bool grounded;
	private Vector3 gravity => new Vector3(0, GravityForce, 0);
	private LayerMask Walkable => LayerMask.GetMask("Ground");

	[Header("Camera Variables")]
	public Transform PlayerBody;
    public Transform CameraTransform;
	[Range(1f, 2f)]
	public float CameraAnchorVerticalOffset;
	[Range(-4f, -8f)]
	public float CameraDistance;
	public Vector2 CrosshairPoint;
	[Range(0.01f, 1f)]
    public float MouseSensitivity;
	private float pitchClamp = 85;
    private float pitch;
	private float yaw;
	private Vector3 CameraAnchorPos => new Vector3(transform.position.x, 
		transform.position.y + CameraAnchorVerticalOffset, 
		transform.position.z);

	[Header("Temporary Debug Variables")]
	bool castingPrimary;
	bool castingSecondary;
	bool castingSpecial;
	bool castingUltimate;

	public Loadout AssignLoadout(LoadoutType SelectedLoadout) => SelectedLoadout switch
    {
        LoadoutType.Ranger => new RangerLoadout(this),
        LoadoutType.Fighter => new FighterLoadout(this),
        _ => throw new System.Exception("Invalid Loadout Value")
    };

    public void Awake()
    {
        //Temporary to avoid Null Reference errors
        AssignedLoadout = AssignLoadout(LoadoutType.Ranger);
	}

    private void Start()
    {
        
	}

	private void OnEnable()
	{
		InputHandler.OnJumpInput += AttemptJump;
	}

	private void Update()
    {
		yaw += InputHandler.MouseDelta.x * MouseSensitivity;
		pitch += -InputHandler.MouseDelta.y * MouseSensitivity;
		pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

		Forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

		Ray groundRay = new Ray(transform.position, Vector3.down);
		if (!Physics.Raycast(groundRay, GroundCheckDistance, Walkable))
		{
			//We are floating, get our ass on the ground.
			grounded = false;
			Velocity += gravity;
		}
		else
		{
			grounded = true;
			Velocity = (Forward * InputHandler.MovementInput.z + transform.right * InputHandler.MovementInput.x) * MoveSpeed;
		}
	}

	public void AttemptJump()
	{
		Debug.Log("tried to jump");
		if (grounded)
		{
			PlayerRB.AddForce(0, JumpForce, 0, ForceMode.Impulse);
		}
	}

	private void FixedUpdate()
	{
		//Lerp MoveSpeed based on Acceleration / Deceleration timers.
		PlayerRB.velocity = Velocity;

		
	}

	private void LateUpdate()
	{
		UpateCameraRotation();
	}

	public void UpateCameraRotation()
    {
		//Body Rotation done seperate from Camera to prevent stutter
		Quaternion desiredYRotation = Quaternion.Euler(0, wrapPi(yaw), 0);
		Quaternion combinedRotation = desiredYRotation;
		PlayerRB.MoveRotation(combinedRotation);

		//Camera rotation
		Quaternion desiredRotation = Quaternion.Euler(pitch, wrapPi(yaw), 0);

		Vector3 rotatedCameraOffset = desiredRotation * new Vector3(0, 0, CameraDistance);
		rotatedCameraOffset += CameraAnchorPos;

		CameraTransform.position = rotatedCameraOffset;

		//Crosshair Point
		Vector3 rotatedCrosshairPoint = desiredRotation * new Vector3(0, CrosshairPoint.y, CrosshairPoint.x);
		rotatedCrosshairPoint += CameraAnchorPos;

		Vector3 dirToLook = (rotatedCrosshairPoint - CameraTransform.position).normalized;

		CameraTransform.rotation = Quaternion.LookRotation(dirToLook);
	}

	public void Interact()
    {

    }

	private void OnDisable()
	{
		InputHandler.OnJumpInput -= AttemptJump;
	}

	//Temporary for Debug Display of Abilities
	private void OnDrawGizmos()
    {
        if (castingPrimary)
        {

        }

        if (castingSecondary)
        {

        }

        if (castingSpecial)
        {

        }

        if (castingUltimate)
        {

        }

		//Velocity vector
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, Velocity);

		//Forward direction the Player will move in
		Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
		Gizmos.DrawRay(transform.position, forward);

		//CameraPos, Anchor, and the Look Point
		Quaternion desiredRotation = Quaternion.Euler(pitch, wrapPi(yaw), 0);

		Matrix4x4 cameraRotationMatrix = Matrix4x4.Rotate(desiredRotation);

		Vector3 rotatedCameraOffset = desiredRotation * new Vector3(0, 0, CameraDistance);
		rotatedCameraOffset += CameraAnchorPos;

		//Crosshair Point
		Vector3 rotatedCrosshairPoint = desiredRotation * new Vector3(0, CrosshairPoint.y, CrosshairPoint.x);
		rotatedCrosshairPoint += CameraAnchorPos;

		Gizmos.color = Color.red;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 0], cameraRotationMatrix[1, 0], cameraRotationMatrix[2, 0]).normalized);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 1], cameraRotationMatrix[1, 1], cameraRotationMatrix[2, 1]).normalized);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 2], cameraRotationMatrix[1, 2], cameraRotationMatrix[2, 2]).normalized);

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(rotatedCameraOffset, 0.25f);

		//Crosshair Look Point
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(rotatedCrosshairPoint, 0.25f);

		//Direction from camera to crosshair point
		Gizmos.DrawRay(CameraTransform.position, (rotatedCrosshairPoint - CameraTransform.position).normalized);

		//Ground ray
		Gizmos.color = Color.white;
		Gizmos.DrawRay(transform.position, Vector3.down * GroundCheckDistance);
	}

	float wrapPi(float theta)
	{
		theta = theta * Mathf.Deg2Rad;

		if (Mathf.Abs(theta) <= Mathf.PI)
		{
			// One revolution is 2 PI.
			const float TWOPI = 2.0f * Mathf.PI;

			// Out of range.  Determine how many "revolutions"
			// we need to add.
			float revolutions = Mathf.Floor((theta + Mathf.PI) * (1.0f / TWOPI));

			// Subtract it off
			theta -= revolutions * TWOPI;
		}

		return theta * Mathf.Rad2Deg;
	}
}
