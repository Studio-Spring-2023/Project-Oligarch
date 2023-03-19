using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageOneHumanoidMelee : MobCore
{
	public Bounds attackHitBox;
	public float attackDistanceOffset;

	[Range(0.25f, 1f)]
	public float rotationStep;
	[Range(0.05f, 0.25f)]
	public float reducedRotationStep;
	[Range(0f, 0.25f)]
	public float angleStepCutoff;



	protected override void Awake()
	{
		base.Awake();

		PrintActionPool(gameObject);
		PrintGoalPool(gameObject);

		MeleeDamage = 4;
	}

	public override bool MoveSelf(GoapAction action)
	{
		//First we get the offset position from our target, since we need space to do an attack
		Vector3 dirFromEntityToTarget = (action.TargetObject.transform.position - transform.position).normalized;
		Vector3 offsetAttackPos = (-dirFromEntityToTarget * attackDistanceOffset) + action.TargetObject.transform.position;

		//Are we in range and is our orientation looking at the Player?
		if ((transform.position - (offsetAttackPos)).sqrMagnitude <= 0.1f)
		{
			Debug.Log("<color=yellow>[StageOneHumanoidMelee]</color>: Made it to action target.");
			
			action.SetInProximity(true);
			EntityNavAgent.velocity = Vector3.zero;
			EntityNavAgent.ResetPath();
			return true;
		}

		Debug.Log($"<color=yellow>[StageOneHumanoidMelee]</color>: Attempting to move Entity towards {action.TargetObject}.");

		if (!EntityNavAgent.hasPath || EntityNavAgent.destination != offsetAttackPos)
		{
			Debug.Log("<color=yellow>[StageOneHumanoidMelee]</color>: Setting Entity NavAgent destination");
			EntityNavAgent.destination = offsetAttackPos;
			EntityNavAgent.velocity = (offsetAttackPos - transform.position).normalized * MoveSpeed;
		}

		//if the difference in our directions is really big, we should perform a different method of slerping that looks a bit cleaner
		Quaternion lookRotation = Quaternion.LookRotation(dirFromEntityToTarget);
		if (Vector3.Dot(transform.forward, dirFromEntityToTarget) <= angleStepCutoff)
		{
			Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, lookRotation, reducedRotationStep);
			transform.rotation = slerpedRotation;
		}
		else
		{
			Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationStep);
			transform.rotation = slerpedRotation;
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
		//Debug Melee Attack View
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(attackHitBox.center, attackHitBox.extents * 2);
	}
}
