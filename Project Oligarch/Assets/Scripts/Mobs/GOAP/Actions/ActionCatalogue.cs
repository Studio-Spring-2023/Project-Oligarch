using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SimpleMeleeAttack : GoapAction
{
	
	bool hasAttacked = false;
	bool isAttacking = false;

	//animatir variable
	public static int action = 0;

	Bounds hitBoxBounds;

	//Debug Timers for now since this will be sync'd with animation
	float totalMeleeTime = 1.25f;
	float meleeTimer;

	public SimpleMeleeAttack(Bounds hitBoxBounds)
    {
		//Here is where we add preconditions and effects
		preconditions.Add(new KeyValuePair<string, object>("playerAlive", true));
		effects.Add(new KeyValuePair<string, object>("playerAlive", false));

		TargetObject = PlayerCore.Transform.gameObject;

		this.hitBoxBounds = hitBoxBounds;

		ActionCost = 4;
	}

	//The biggest and most important function on an action. This is called by the "PerformAction"
	//state and does the unique behavior that this action has tied to it. 
	public override bool PerformAction(MobCore entity)
	{
        //Attack the player!
        entity.Action = action;
        if (CanAttack() && !isAttacking)
		{
			isAttacking = true;
			
			//animation action variable
			action = 1;

			if (Physics.CheckBox(entity.transform.localToWorldMatrix.MultiplyPoint3x4(hitBoxBounds.center), hitBoxBounds.extents, entity.transform.rotation, PlayerMask))
			{
				PlayerCore.Damaged(entity.Damage);
				//Could add a goal state here that we hit the player, so we could make them do a combo attack after?
			}
		}
        entity.Action = action;
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
        entity.Action = action;
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

		//animation action variable
		action = 0;
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

public class SimpleRangedAttack : GoapAction
{
	bool hasFired;

	bool isFiring;

	int shotsFired;

	float shotCooldownTimer;
	float shotCooldown = 0.75f;

	//animatir variable
	public static int action = 0;

	public SimpleRangedAttack()
	{
		preconditions.Add(new KeyValuePair<string, object>("playerAlive", true));
		effects.Add(new KeyValuePair<string, object>("playerAlive", false));

		TargetObject = PlayerCore.Transform.gameObject;

		ActionCost = 2;
	}

	public override bool CheckProceduralPrecondition(MobCore entity)
	{
        entity.Action = action;
        return true;
	}

	public override bool isDone()
	{
		return hasFired;
	}

	public override bool MustBeInProximity()
	{
		return true;
	}

	public override bool PerformAction(MobCore entity)
	{
        entity.Action = action;
        if (CanFire() && !isFiring)
		{
			shotsFired++;
			isFiring = true;

			//animation action variable
			action = 1;

			Ray ray = new Ray(entity.transform.position, (TargetObject.transform.position - entity.transform.position).normalized);
			if (Physics.Raycast(ray, entity.MaxMovementOffset, PlayerMask))
			{
				PlayerCore.Damaged(entity.Damage);
				//Could add a goal state here that we hit the player, so we could make them do a combo attack after?
			}

			if (shotsFired > 5)
				hasFired = true;
		}

		if ((TargetObject.transform.position - entity.transform.position).magnitude > entity.MaxMovementOffset)
			SetInProximity(false);

		if ((TargetObject.transform.position - entity.transform.position).magnitude < entity.MinMovementOffset)
		{
			entity.MoveSelf(this);
		}
        entity.Action = action;
        return true;
	}

	private bool CanFire()
	{
		if (isFiring)
		{
			if (shotCooldownTimer > shotCooldown)
			{
				shotCooldownTimer = 0;
				isFiring = false;
				return false;
			}
			else
			{
				shotCooldownTimer += Time.deltaTime;
				return false;
			}
		}

		return true;
	}

	protected override void OnReset()
	{
		hasFired = false;
		shotsFired = 0;
		shotCooldownTimer = 0;

		//animation action variable
		action = 0;
	}
}

public class SimpleThrow : GoapAction
{

	//animatir variable
	public static int action = 0;

	bool hasThrown;

	bool tryingToThrow;

	float throwingDurationTimer;
	float throwingDuration = 0.75f;

	float checkRadius;
	float throwSpeed;

	Vector3 throwPos;

	LayerMask Throwables;

	public SimpleThrow(float radiusToCheck, float throwSpeed, Vector3 throwPos)
	{
        preconditions.Add(new KeyValuePair<string, object>("playerAlive", true));
		effects.Add(new KeyValuePair<string, object>("playerAlive", false));

		TargetObject = null;
		checkRadius = radiusToCheck;
		Throwables = LayerMask.GetMask("Throwable");

		this.throwPos = throwPos;
		this.throwSpeed = throwSpeed;
		ActionCost = 1;
	}

	public override bool CheckProceduralPrecondition(MobCore entity)
	{
        entity.Action = action;
        Collider[] foundThrowables = Physics.OverlapSphere(entity.transform.position, checkRadius, Throwables);

		if (foundThrowables == null)
			return false;

		if (foundThrowables.Length == 1)
		{
			TargetObject = foundThrowables[0].gameObject;
			return true;
		}

		float shortestDistance = 100;
		foreach (Collider throwable in foundThrowables)
		{
			float distance = (entity.transform.position - throwable.transform.position).sqrMagnitude;
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				TargetObject = throwable.gameObject;
			}
		}
        entity.Action = action;
        return (TargetObject != null);
	}

	public override bool isDone()
	{
		return hasThrown;
	}

	public override bool MustBeInProximity()
	{
		return true;
	}

	public override bool PerformAction(MobCore entity)
	{
        entity.Action = action;
        //THROW THE BARREL AT THE PLAYER
        if (CanThrow() && !tryingToThrow)
		{
			Vector3 adjustedThrowPos = throwPos + entity.transform.position;

			tryingToThrow = true;

			//animation action variable
			action = 2;

			Vector3 dirFromEntityToPlayer = PlayerCore.Transform.position - adjustedThrowPos;
			float? angle = CalculateAngle(dirFromEntityToPlayer);

			if (angle == null)
			{
				Debug.Log("Failed to get proper angle for throw");
				return false;
			}

			TargetObject.transform.position = adjustedThrowPos;
			Quaternion velocityRotation = Quaternion.Euler((float)angle, 0, 0);
			Vector3 throwVelocity = (velocityRotation * dirFromEntityToPlayer.normalized) * throwSpeed;
			Rigidbody TargetRB = TargetObject.GetComponent<Rigidbody>();
			TargetRB.constraints = RigidbodyConstraints.None;
			TargetRB.velocity = throwVelocity;
		}
        entity.Action = action;
        return true;
	}

	bool CanThrow()
	{
		if (tryingToThrow)
		{
			if (throwingDurationTimer > throwingDuration)
			{
				hasThrown = true;
				return false;
			}
			else
			{
				throwingDurationTimer += Time.deltaTime;
				return false;
			}
		}

		return true;
	}

	float? CalculateAngle(Vector3 dir)
	{
		float speedSquared = throwSpeed * throwSpeed;
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

	

	protected override void OnReset()
	{
		hasThrown = false;
		throwingDurationTimer = 0;
		TargetObject = null;
		tryingToThrow = false;

		//animation action variable
		action = 0;
	}
}
