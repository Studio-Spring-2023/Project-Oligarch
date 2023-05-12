using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidSimpleHulking : MobCore
{
	//Animator Reference
	public Animator hulkAnim;

	public void FixedUpdate()
	{
		hulkAnim.SetInteger("action", Action);
	}

	public Bounds attackHitBox;

	[Range(5f, 30f)]
	public float ThrowStrength;
	[Range(3f, 15f)]
	public float ThrowableCheckRadius;
	public Vector3 ThrowablePos;

	[Range(0.25f, 1f)]
	public float rotationStep;
	

	protected override void Awake()
	{
		base.Awake();

		PrintActionPool(gameObject);
		PrintGoalPool(gameObject);
	}

	public override bool MoveSelf(GoapAction action)
	{
		//First we get the offset position from our target, since we need space to do an attack
		Vector3 dirFromEntityToTarget = (action.TargetObject.transform.position - transform.position).normalized;
		Vector3 offsetAttackPos = (-dirFromEntityToTarget * MaxMovementOffset) + action.TargetObject.transform.position;

		//Are we in range and is our orientation looking at the Player?
		if ((offsetAttackPos - transform.position).sqrMagnitude <= 0.01f)
		{
			Debug.Log("<color=yellow>[HumanoidSimpleMelee]</color>: Made it to action target.");

			action.SetInProximity(true);
			EntityNavAgent.velocity = Vector3.zero;
			EntityNavAgent.ResetPath();
			return true;
		}

		Debug.Log($"<color=yellow>[HumanoidSimpleMelee]</color>: Attempting to move Entity towards {action.TargetObject}.");

		//Can we see the Player? If not, move through our points.
		if (!EntityNavAgent.Raycast(offsetAttackPos, out NavMeshHit hit))
		{
			EntityNavAgent.velocity = dirFromEntityToTarget.normalized * 4f;
			Quaternion lookRotation = Quaternion.LookRotation(dirFromEntityToTarget);
			Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationStep);
			transform.rotation = slerpedRotation;
		}
		else
		{
			//We cannot see the Player, so move through our position points.
			Quaternion lookRotation = Quaternion.LookRotation(EntityNavAgent.velocity.normalized);
			Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationStep);
			transform.rotation = slerpedRotation;
		}

		if (EntityNavAgent.destination != offsetAttackPos)
		{
			//We don't have a path, and our destination is not at the right spot (meaning the Player has moved).
			//This becomes a performance cap as it is synchronous and we should really only calculate a new path
			//if the entity is within a close enough range where it matters or the player has moved far enough away
			//from agents original destination
			if (EntityNavAgent.CalculatePath(offsetAttackPos, EntityNavPath))
				EntityNavAgent.SetPath(EntityNavPath);
			else Debug.Log($"<color=red>[HumanoidSimpleMelee]</color>: Failed to generate Entity Nav Path.");
		}

		return false;
	}

	protected override void GenerateAvailableEntityActions()
	{
		availableEntityActionPool = new HashSet<GoapAction>();
		availableEntityActionPool.Add(new SimpleMeleeAttack(attackHitBox));
		availableEntityActionPool.Add(new SimpleThrow(ThrowableCheckRadius, ThrowStrength, ThrowablePos));
	}

	protected override void GenerateStartingGoalState()
	{
		currentGoalPool.Add(new KeyValuePair<string, object>("playerAlive", false));
	}

	private void OnDrawGizmos()
	{
		if (EntityNavPath != null)
		{
			Vector3 lastCorner = EntityNavPath.corners[0];
			Gizmos.DrawLine(transform.position, lastCorner);
			foreach (Vector3 corner in EntityNavPath.corners)
			{
				Gizmos.DrawLine(lastCorner, corner);
				lastCorner = corner;
			}
		}

		if (Application.isPlaying)
		{
			Gizmos.color = Color.yellow;
			Vector3 dirToPlayer = (PlayerCore.Transform.position - (ThrowablePos + transform.position));
			Gizmos.DrawRay(ThrowablePos + transform.position, dirToPlayer);
			float? angle = CalculateAngle(dirToPlayer);
			//Debug.Log(angle);
			if (angle != null)
			{
				Gizmos.color = Color.red;
				Quaternion rotation = Quaternion.Euler((float)angle, 0, 0);
				Vector3 throwVelocity = (rotation * dirToPlayer.normalized) * ThrowStrength;
				Gizmos.DrawRay(ThrowablePos + transform.position, throwVelocity);
			}
			else
			{
				Debug.Log("That shit null bruh");
			}
		}

		//Debug Melee Attack View
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(ThrowablePos, 0.25f);
		Gizmos.DrawWireCube(attackHitBox.center, attackHitBox.extents * 2);
		Gizmos.DrawWireSphere(Vector3.zero, ThrowableCheckRadius);
	}

	float? CalculateAngle(Vector3 dir)
	{
		float speedSquared = ThrowStrength * ThrowStrength;
		float y = dir.y;
		dir.y = 0;
		float x = dir.magnitude;
		float gravity = -GameManager.Gravity;
		float underRoot = (speedSquared * speedSquared) - gravity * (gravity * x * x + 2 * y * speedSquared);

		if (underRoot >= 0)
		{
			float root = Mathf.Sqrt(underRoot);
			float angle = speedSquared - root;

			return (Mathf.Atan2(angle, gravity * x) * Mathf.Rad2Deg);
		}
		else
		{
			return null;
		}
	}
}
