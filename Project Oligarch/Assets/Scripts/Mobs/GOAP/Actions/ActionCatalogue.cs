using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMeleeAttack : GoapAction
{
	bool hasAttacked = false;
	bool isAttacking = false;

	Bounds hitBoxBounds;

	//Debug Timers for now since this will be sync'd with animation
	float totalMeleeTime = 2f;
	float meleeTimer;

	public SimpleMeleeAttack(Bounds hitBoxBounds)
    {
		//Here is where we add preconditions and effects
		preconditions.Add(new KeyValuePair<string, object>("playerAlive", true));
		effects.Add(new KeyValuePair<string, object>("playerAlive", false));

		TargetObject = PlayerCore.Transform.gameObject;

		this.hitBoxBounds = hitBoxBounds;
	}

	//The biggest and most important function on an action. This is called by the "PerformAction"
	//state and does the unique behavior that this action has tied to it. 
	public override bool PerformAction(MobCore entity)
	{
		//Attack the player!

		if (CanAttack() && !isAttacking)
		{
			isAttacking = true;

			if (Physics.CheckBox(entity.transform.localToWorldMatrix.MultiplyPoint3x4(hitBoxBounds.center), hitBoxBounds.extents, entity.transform.rotation, PlayerMask))
			{
				PlayerCore.Damaged(entity.MeleeDamage);
				//Could add a goal state here that we hit the player, so we could make them do a combo attack after?
			}
		}	

		return true;
	}

	private bool CanAttack()
	{
		if (isAttacking)
		{
			if (meleeTimer > totalMeleeTime)
			{
				hasAttacked = true;
				return false;
			}
			else
			{
				meleeTimer += Time.deltaTime;
				return false;
			}
		}

		return true;
	}

	//This is where we can check the current state of the envrionment around the entity to ensure 
	//that if there's anything specific that it needs or can use is within a reasonable range,
	//otherwise this action will be removed from the possibilites by the planner
	public override bool CheckProceduralPrecondition(MobCore entity)
	{
		//Can do world state checks here!
		return true;
	}

	//Some actions may need to have their internal state reset, so we do that here.
	//Maybe thats a timer, the target the action has tied to it, or anything else
	//that we want it to clear from itself.
	protected override void OnReset()
	{
		hasAttacked = false;
		isAttacking = false;
		meleeTimer = 0;
	}

	//Used by the FSM to keep track of whether the action it's been performing is complete or not.
	//Once isDone is true, this action gets pop'd off the stack and the next action begins. If there is
	//no actions left, then the FSM returns to Idle and creates a new plan.
	public override bool isDone()
	{
		return hasAttacked;
	}

	//An easy helper function that we can call that lets us know whether this
	//action requires the entity to be within a certain range of something. We use this
	//so we know to move the entity towards the proximity it needs to be within before we try
	//to perform the action
	public override bool MustBeInProximity()
	{
		return true;
	}
}

public class RangeAttackPlayer : GoapAction
{
	public RangeAttackPlayer()
	{
		//Here is where we add preconditions and effects
	}

	public override bool CheckProceduralPrecondition(MobCore entity)
	{
		throw new System.NotImplementedException();
	}

	public override bool isDone()
	{
		throw new System.NotImplementedException();
	}

	public override bool MustBeInProximity()
	{
		throw new System.NotImplementedException();
	}

	public override bool PerformAction(MobCore entity)
	{
		throw new System.NotImplementedException();
	}

	protected override void OnReset()
	{
		throw new System.NotImplementedException();
	}
}
