using System.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCore : Core
{
	public static Transform Transform { get; private set; }
    public static Loadout AssignedLoadout { get; private set; }

    [Header ("Slide Variables")]
	public bool canSlide;
	public bool Slide;
	public float SlideTime;
	public float SlideCooldown;
	public float SlideForce;

	[Header("Movement Variables")]
    private Rigidbody PlayerRB;
	private Vector3 Velocity;
    private Vector3 Forward;
    private Vector3 Right;
    private Vector3 temp;
    [Range(5f, 15f)]
    public float AirStrafeSpeed;
    [Range(1f, 20f)]
	public static float JumpForce;
	public float GroundCheckDistance;
	private bool grounded;
	private Vector3 gravity => new Vector3(0, GameManager.Gravity, 0);
	private LayerMask Walkable => LayerMask.GetMask("Ground");

	[Header("Camera Variables")]
	public Transform PlayerBody;
    public Transform CameraTransform;
	[HideInInspector]
	public Vector3 lookDir;
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
	public Vector3 RotatedCrosshairPoint => (DesiredRotation * CrosshairPoint) + CameraAnchorPos;
	private Vector3 CameraCollisionOffset => CameraTransform.forward * CameraIntersetOffsetDistance;

	[Header("Interact Variables")]
	public float InterCheckMaxDistance;
	public LayerMask Interactables;

	[Header("Crosshair Variables")]
	public RectTransform Crosshair;

	private bool jump;
	public TakeHealthDamage HP;
	//zach animation ref
	public Animator playerRanged;

	public Loadout AssignLoadout(LoadoutType SelectedLoadout) => SelectedLoadout switch
    {
        LoadoutType.Ranger => new RangerLoadout(this),
        LoadoutType.Fighter => new FighterLoadout(this),
        _ => throw new System.Exception("Invalid Loadout Value")
    };

    public void Awake()
    {
		PlayerRB = GetComponent<Rigidbody>();

		Transform = transform;

        //Temporary to avoid Null Reference errors
        AssignedLoadout = AssignLoadout ( LoadoutType.Ranger );

		MoveSpeed = 6f;
	}

    private void Start()
    {
		SprintSpeed *= MoveSpeed;
        StartSpeed = MoveSpeed;
	}

	private void OnEnable()
	{
		InputHandler.OnJumpInput += AttemptJump;
		InputHandler.OnInteractInput += Interact;
    }
    float ydirection;
    float xdirection;
    bool sprinting = false;
    
	private void Update()
    {
        //animation controls

        Forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
		//

        yaw += InputHandler.MouseDelta.x * MouseSensitivity;
		pitch += -InputHandler.MouseDelta.y * MouseSensitivity;
		pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        Forward = new Vector3 ( transform.forward.x , 0 , transform.forward.z ).normalized;

		Ray groundRay = new Ray(transform.position, Vector3.down);
		if (!Physics.Raycast(groundRay, GroundCheckDistance, Walkable))
		{
			//We are floating, get our ass on the ground.
			grounded = false;
		}
		else
		{
			grounded = true;
			

		}
		if(Input.GetButtonDown("Ability1") && canSlide)
		{
			StartCoroutine(SlideFunc());
		}
		if(Input.GetButtonDown("Sprint") && ydirection > 0)
		{
			MoveSpeed = SprintSpeed;

            sprinting = true;
            ydirection = Input.GetAxis("Vertical") + 1f;
            xdirection = Input.GetAxis("Horizontal");
            playerRanged.SetFloat("x", xdirection);
            playerRanged.SetFloat("y", ydirection);

        }
		else if(Input.GetButtonUp("Sprint"))
		{
			MoveSpeed = StartSpeed;
			sprinting = false;

        }
		if (sprinting == false)
		{
            ydirection = Input.GetAxis("Vertical");
            xdirection = Input.GetAxis("Horizontal");
            playerRanged.SetFloat("x", xdirection);
            playerRanged.SetFloat("y", ydirection);
        }
	}

	public void AttemptJump()
	{
		if (!jump)
		{
			jump = true;
		}
	}

	public IEnumerator SlideFunc()
	{
		Debug.Log("Slide");
		playerRanged.SetInteger("Actions", 2);
		canSlide = false;
		temp = (Forward * InputHandler.MovementInput.z + transform.right * InputHandler.MovementInput.x);
        Slide = true;
		//Renderer render = gameObject.GetComponent<Renderer>();
        //render.material.SetColor("_Color", Color.red);
        //PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, -20f, PlayerRB.velocity.z);
		float StartSpeed = MoveSpeed;
        MoveSpeed *= SlideForce;
        //PlayerRB.AddForce(temp.normalized, ForceMode.Force);
        yield return new WaitForSeconds(SlideTime);
		playerRanged.SetInteger("Actions", 0);
		Slide = false;
        //render.material.SetColor("_Color", Color.grey);
        MoveSpeed = StartSpeed;
        yield return new WaitForSeconds(SlideCooldown);
        canSlide = true;
        yield return null;  
    }

	private void FixedUpdate()
	{
		if (grounded && !Slide)
		{
            Velocity = ( Forward * InputHandler.MovementInput.z + Right * InputHandler.MovementInput.x ) * MoveSpeed;
            playerRanged.SetInteger("Actions", 0);
		}
		else if(!grounded)
		{
            Vector3 flatVelocity = (Forward * InputHandler.MovementInput.z + Right * InputHandler.MovementInput.x) * AirStrafeSpeed;
            Velocity = new Vector3(flatVelocity.x, Velocity.y, flatVelocity.z);
            Velocity += gravity;
        }
		if(Slide)
		{
			Velocity = (temp * MoveSpeed);
		}

		if (jump)
		{
			playerRanged.SetInteger("Actions", 1);
            Velocity += Vector3.up * JumpForce;
			jump = false;
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
		{
			//Debug.Log("Object Obstructing View");
			Vector3 CameraCollisionOffset = CameraTransform.forward * CameraIntersetOffsetDistance;

			CameraTransform.position = collidedObj.point + CameraCollisionOffset;
		}

		//Look in the direction of the crosshair
		CameraTransform.rotation = Quaternion.LookRotation(dirFromCameraToCrosshair.normalized);
	}

	//This will be moved to UIHandler, but for now I put it here since it was easier
	//TODO: Move this to UIHandler
	public void DrawCrosshair()
	{
		Vector2 CrosshairScreenSpace = UnityEngine.Camera.main.WorldToScreenPoint(RotatedCrosshairPoint);
		Crosshair.transform.position = CrosshairScreenSpace;
	}

	public void Interact()
    {
		lookDir = RotatedCrosshairPoint - CameraTransform.position;

		Ray interRay = new Ray(CameraTransform.position, lookDir);
		if (Physics.Raycast(interRay, out RaycastHit hit, InterCheckMaxDistance, Interactables))
			hit.transform.gameObject.GetComponent<Interactable>().InteractedWith();
    }

	public static void Damaged(int damage)
	{
		TakeHealthDamage.TakeDamage(damage);		
	}

	private void OnDisable()
	{
		InputHandler.OnJumpInput -= AttemptJump;
		InputHandler.OnInteractInput -= Interact;
	}

	//Temporary for Debug Display of Abilities
	private void OnDrawGizmos()
    {
		//Velocity vector
        //Gizmos.color = Color.white;
        //Gizmos.DrawRay(transform.position, Velocity);

		//Forward direction the Player will move in
		//Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
		//Gizmos.DrawRay(transform.position, forward);

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
