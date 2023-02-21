using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCore : Core
{
	public static Loadout AssignedLoadout { get; private set; }

	[Header("Movement Variables")]
    public Rigidbody PlayerRB;
	private Vector3 Velocity;
    private Vector3 Forward;

    [Header("Camera Variables")]
    public Vector3 CrosshairLook;
    [Range(0.01f, 1f)]
    public float MouseSensitivity;
	[Range(0, 90)]
	public float PitchClamp;
    private float pitch;
	private float yaw;

	[Header("Temporary Debug Variables")]
	bool castingPrimary;
	bool castingSecondary;
	bool castingSpecial;
	bool castingUltimate;

	public Loadout AssignLoadout(LoadoutType SelectedLoadout) => SelectedLoadout switch
    {
        LoadoutType.Ranger => new RangerLoadout(this),
        LoadoutType.Fighter => new FighterLoadout(this),
        _ => throw new System.Exception("Invalid Loadout Enum Value")
    };

    public void Awake()
    {
        //Temporary to avoid Null Reference errors
        AssignedLoadout = AssignLoadout(LoadoutType.Ranger);
	}

    private void Start()
    {
        
	}

    private void Update()
    {
        yaw += InputHandler.MouseDelta.x * MouseSensitivity;
        pitch += -InputHandler.MouseDelta.y * MouseSensitivity;
		pitch = Mathf.Clamp(pitch, -PitchClamp, PitchClamp);

		transform.rotation = Quaternion.Euler(pitch, yaw, 0);

		Forward = new Vector3(transform.forward.x, 0, transform.forward.z);
	}

	private void FixedUpdate()
	{
        //Lerp MoveSpeed based on Acceleration / Deceleration timers.
		Velocity = (Forward.normalized * InputHandler.MovementInput.z + transform.right * InputHandler.MovementInput.x) * MoveSpeed;

		PlayerRB.velocity = Velocity;
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

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Velocity);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + CrosshairLook, 0.25f);

		Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
		Gizmos.DrawRay(transform.position, forward);
	}
}
