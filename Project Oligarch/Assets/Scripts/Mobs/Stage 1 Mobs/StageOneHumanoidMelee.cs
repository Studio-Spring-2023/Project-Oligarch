using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneHumanoidMelee : MobCore
{

	protected override void Awake()
	{
		base.Awake();

		PrintActionPool(gameObject);
		PrintGoalPool(gameObject);
	}

	protected override void CreateIdleState()
	{
		Idle = (FSM, gameObject) =>
		{
			Stack<GoapAction> plan = planner.BuildPlan(gameObject, availableEntityActionPool, MobManager.GetWorldProperties(), currentGoalPool);
			if (plan != null)
			{
				currentEntityActions = plan;
				PlanFound(currentGoalPool, currentEntityActions);
			}
			else
			{
				PlanFailed(currentGoalPool);
			}
		};
	}

	protected override void CreateMoveToState()
	{
		MoveTo = (FSM, gameObject) =>
		{
			Debug.Log("Stage One Humanoid Melee MoveTo");
		};
	}

	protected override void CreatePerformActionState()
	{
		PerformAction = (FSM, gameObject) =>
		{
			Debug.Log("Stage One Humanoid Melee Perform Action");
		};
	}

	protected override void GenerateAvailableEntityActions()
	{
		availableEntityActionPool = new HashSet<GoapAction>();
		availableEntityActionPool.Add(new MeleeAttackPlayer());
	}

	public override bool MoveSelf(GoapAction nextAction)
	{


		return true;
	}

	protected override void GenerateStartingGoalState()
	{
		currentGoalPool.Add(new KeyValuePair<string, object>("playerAlive", false));
	}
}
