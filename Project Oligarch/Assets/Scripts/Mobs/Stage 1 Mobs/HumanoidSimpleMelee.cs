using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidSimpleMelee : MobCore
{
	//Animator Reference
	public Animator meleeAnim;
	

	public Bounds attackHitBox;

	[Range(0.25f, 1f)]
	public float rotationStep;
    //[Range(0.05f, 0.25f)]
    //public float reducedRotationStep;
    //[Range(0f, 0.25f)]
    //public float angleStepCutoff;
    public void FixedUpdate()
    {
		meleeAnim.SetInteger("action", Action);
	}
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
			EntityNavAgent.velocity = dirFromEntityToTarget.normalized * MoveSpeed;
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
	}

	protected override void GenerateStartingGoalState()
	{
		currentGoalPool.Add(new KeyValuePair<string, object>("playerAlive", false));
	}

	private void OnDrawGizmos()
	{
		//if (EntityNavPath != null)
		//{
		//	//Vector3 lastCorner = EntityNavPath.corners[0];
		//	Gizmos.DrawLine(transform.position, lastCorner);
		//	foreach (Vector3 corner in EntityNavPath.corners)
		//	{
		//		Gizmos.DrawLine(lastCorner, corner);
		//		lastCorner = corner;
		//	}
		//}

		//Debug Melee Attack View
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(attackHitBox.center, attackHitBox.extents * 2);
	}
}
