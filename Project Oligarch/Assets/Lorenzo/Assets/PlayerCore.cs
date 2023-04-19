using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCore : Core
{
	public static Transform Transform { get; private set; }
	public static Loadout CurrentLoadout { get; private set; }

	[Header("Movement Variables")]
    private Rigidbody PlayerRB;
	private Vector3 Velocity;
    private Vector3 Forward;
	private Vector3 Right;
	[Range(1.25f, 2f)]
	public float SprintMultiplier;
	[Range(1f, 25f)]
	public float JumpForce;
	[Range(5f, 15f)]
	public float AirStrafeSpeed;
	public float GroundCheckDistance;
	[Range(25f, 65f)]
	public float MaxSlopeAngle;
	public float SlopeCheckDistance;
	private bool inAir;
	private bool isSprinting;
	private bool canJump;
	private int jumpCharges;
	private int maxJumps => (int)ItemManager.Instance.JumpChargeBonus + CurrentLoadout.BaseJumpCharges;
	private Vector3 gravity => new Vector3(0, GameManager.Gravity, 0);
	private LayerMask Walkable => LayerMask.GetMask("Ground");

	[Header("Camera Variables")]
	public Transform PlayerBody;
    public Transform CameraTransform;
	[Range(1f, 2f)]
	public float CameraAnchorVerticalOffset;
	[Range(-4f, -8f)]
	public float CameraDistance;
	public Vector3 CrosshairPoint;
	[Range(0.01f, 1f)]
    public float MouseSensitivity;
	[Range(1f, 5f)]
	public float CameraIntersectionCheckDistance;
	[Range(1f, 5f)]
	public float CameraIntersetOffsetDistance;
	private float pitchClamp = 85;
    private float pitch;
	private float yaw;
	private Vector3 CameraAnchorPos => new Vector3(transform.position.x, 
		transform.position.y + CameraAnchorVerticalOffset, 
		transform.position.z);
	private Quaternion DesiredRotation => Quaternion.Euler(pitch, wrapPi(yaw), 0);
	private Vector3 RotatedCrosshairPoint => (DesiredRotation * CrosshairPoint) + CameraAnchorPos;
	private Vector3 CameraCollisionOffset => CameraTransform.forward * CameraIntersetOffsetDistance;

	[Header("Interact Variables")]
	public float InterCheckMaxDistance;
	public LayerMask Interactables;

	[Header("Crosshair Variables")]
	public RectTransform Crosshair;

	public Loadout AssignLoadout(LoadoutType SelectedLoadout) => SelectedLoadout switch
    {
        LoadoutType.Ranger => new RangerLoadout(this),
        LoadoutType.Fighter => new FighterLoadout(this),
        _ => throw new System.Exception("Invalid Loadout Value")
    };

    public void Awake()
    {
		CurrentLoadout = AssignLoadout(LoadoutType.Ranger);

		PlayerRB = GetComponent<Rigidbody>();

		Transform = transform;
	}

	private void OnEnable()
	{
		InputHandler.OnJumpInput += AttemptJump;
		InputHandler.OnInteractInput += Interact;
		InputHandler.OnSprintInput += OnSprint;
	}

	private void Update()
    {
		yaw += InputHandler.MouseDelta.x * MouseSensitivity;
		pitch += -InputHandler.MouseDelta.y * MouseSensitivity;
		pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

		Ray groundRay = new Ray(transform.position, Vector3.down);
		
		if (Physics.Raycast(groundRay, GroundCheckDistance, Walkable) && !canJump)
		{
			jumpCharges = maxJumps;

			inAir = false;

			Forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
			Right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

			//We do a different check for the slope since the character model might be slightly different and the collider may cause issues where the slope
			if (Physics.Raycast(groundRay, out RaycastHit walkableHit, SlopeCheckDistance, Walkable))
			{
				float angle = Vector3.Angle(Vector3.up, walkableHit.normal);

				if (angle < MaxSlopeAngle)
				{
					Forward = Vector3.Cross(transform.right, walkableHit.normal).normalized;
					Right = Vector3.Cross(walkableHit.normal, Forward).normalized;
				}
			}
		}
		else
		{
			//We are floating, get on ground.
			inAir = true;

			Forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
			Right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
		}
	}

	public void OnSprint()
	{
		isSprinting = !isSprinting;
	}

	public void AttemptJump()
	{
		if (jumpCharges >= maxJumps)
		{
			jumpCharges--;

			canJump = true;
		}
	}

	private void FixedUpdate()
	{
		if (!inAir)
		{
			Velocity = (Forward * InputHandler.MovementInput.z + Right * InputHandler.MovementInput.x);

			if (isSprinting)
				Velocity *= (MoveSpeed * SprintMultiplier);
			else
				Velocity *= MoveSpeed;
		}
		else
		{
			Vector3 flatVelocity = (Forward * InputHandler.MovementInput.z + Right * InputHandler.MovementInput.x) * AirStrafeSpeed;
			Velocity = new Vector3(flatVelocity.x, Velocity.y, flatVelocity.z);

			Velocity += gravity;
		}

		if (canJump)
		{
			if (isSprinting)
				Velocity += Vector3.up * (JumpForce * SprintMultiplier);
			else
				Velocity += Vector3.up * JumpForce;

			canJump = false;
		}

		//Lerp MoveSpeed based on Acceleration / Deceleration timers.
		PlayerRB.velocity = Velocity;
	}

	private void LateUpdate()
	{
		UpateCameraRotation();
		DrawCrosshair();
	}

	public void UpateCameraRotation()
    {
		//Body Rotation done seperate from Camera to prevent stutter
		Quaternion desiredYRotation = Quaternion.Euler(0, wrapPi(yaw), 0);
		PlayerRB.MoveRotation(desiredYRotation);

		//Camera Position Rotation
		Vector3 rotatedCameraOffsetPos = (DesiredRotation * new Vector3(0, 0, CameraDistance)) + CameraAnchorPos;
		CameraTransform.position = rotatedCameraOffsetPos;

		//After rotating, check for intersections between the camera and the world
		Vector3 dirFromCameraToCrosshair = (RotatedCrosshairPoint - CameraTransform.position);

		//Want to check from the camera anchor position, because we only want to move the camera if an object intersects between that point
		//and our camera

		//TODO: Change this to shoot the ray from the Player's head, because you want to see your character
		//if the character isnt in your view port, then that's a problem
		Vector3 adjustedCameraAnchor = (dirFromCameraToCrosshair * 0.5f);
		Ray hitRay = new Ray(rotatedCameraOffsetPos + adjustedCameraAnchor, -adjustedCameraAnchor.normalized);

		if (Physics.Raycast(hitRay, out RaycastHit collidedObj, adjustedCameraAnchor.magnitude * CameraIntersectionCheckDistance))
			CameraTransform.position = collidedObj.point + CameraCollisionOffset;

		//Look in the direction of the crosshair
		CameraTransform.rotation = Quaternion.LookRotation(dirFromCameraToCrosshair.normalized);
	}

	//This will be moved to UIHandler, but for now I put it here since it was easier
	//TODO: Move this to UIHandler
	public void DrawCrosshair()
	{
		Vector2 CrosshairScreenSpace = Camera.main.WorldToScreenPoint(RotatedCrosshairPoint);
		Crosshair.transform.position = CrosshairScreenSpace;
	}

	public void Interact()
    {
		Vector3 lookDir = RotatedCrosshairPoint - CameraTransform.position;

		Ray interRay = new Ray(CameraTransform.position, lookDir);
		if (Physics.Raycast(interRay, out RaycastHit hit, InterCheckMaxDistance, Interactables))
			hit.transform.gameObject.GetComponent<Interactable>().InteractedWith();
    }

	public static void Damaged(int damage)
	{
		Debug.Log($"<color=green>[PlayerCore]</color>: Player took {damage} damage.");
	}

	private void OnDisable()
	{
		InputHandler.OnJumpInput -= AttemptJump;
		InputHandler.OnInteractInput -= Interact;
		InputHandler.OnSprintInput -= OnSprint;
	}

	//Temporary for Debug Display of Abilities
	private void OnDrawGizmos()
    {
		//Velocity vector
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Velocity);

		//Sloped Walking
		Ray groundRay = new Ray(transform.position, Vector3.down);

		if (Physics.Raycast(groundRay, out RaycastHit walkableHit, 10f, Walkable))
		{
			Gizmos.color = new Color(255f, 205f, 129f);
			Gizmos.DrawRay(walkableHit.point, walkableHit.normal);

			float angle = Vector3.Angle(Vector3.up, walkableHit.normal);

			if (angle < MaxSlopeAngle)
			{
				Vector3 SlopedForward = Vector3.Cross(transform.right, walkableHit.normal).normalized;
				Vector3 SlopedRight = Vector3.Cross(walkableHit.normal, SlopedForward).normalized;

				Gizmos.color = Color.green;
				Gizmos.DrawRay(transform.position, SlopedForward);
				Gizmos.DrawRay(transform.position, SlopedRight);
			}
		}

		//CameraPos, Anchor, and the Look Point
		Quaternion desiredRotation = Quaternion.Euler(pitch, wrapPi(yaw), 0);

		Matrix4x4 cameraRotationMatrix = Matrix4x4.Rotate(desiredRotation);

		Vector3 rotatedCameraOffset = desiredRotation * new Vector3(0, 0, CameraDistance);
		rotatedCameraOffset += CameraAnchorPos;

		Gizmos.color = Color.red;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 0], cameraRotationMatrix[1, 0], cameraRotationMatrix[2, 0]).normalized);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 1], cameraRotationMatrix[1, 1], cameraRotationMatrix[2, 1]).normalized);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(CameraAnchorPos, new Vector3(cameraRotationMatrix[0, 2], cameraRotationMatrix[1, 2], cameraRotationMatrix[2, 2]).normalized);

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(rotatedCameraOffset, 0.25f);
		if (!Application.isPlaying)
			CameraTransform.position = rotatedCameraOffset;

		//Crosshair Look Point
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(RotatedCrosshairPoint, 0.25f);

		//Ground ray
		Gizmos.color = Color.white;
		Gizmos.DrawRay(transform.position, Vector3.down * GroundCheckDistance);

		//Interact Ray
        Vector3 dirFromCameraToCrosshair = RotatedCrosshairPoint - CameraTransform.position;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(RotatedCrosshairPoint, dirFromCameraToCrosshair.normalized * 1.25f);

		//Camera Intersection Test
		Gizmos.color = Color.yellow;
		Vector3 adjustedCameraAnchor = dirFromCameraToCrosshair * 0.5f;
		Gizmos.DrawRay(rotatedCameraOffset + adjustedCameraAnchor, -adjustedCameraAnchor * CameraIntersectionCheckDistance);
	}

	float wrapPi(float degrees)
	{
		float radians = degrees * Mathf.Deg2Rad;

		if (Mathf.Abs(degrees) <= Mathf.PI)
		{
			// One revolution is 2 PI.
			const float TWOPI = 2.0f * Mathf.PI;

			// Out of range.  Determine how many "revolutions" we need to add.
			float revolutions = Mathf.Floor((radians + Mathf.PI) * (1.0f / TWOPI));

			// Subtract it off
			radians -= revolutions * TWOPI;
		}

		return radians * Mathf.Rad2Deg;
	}
}
