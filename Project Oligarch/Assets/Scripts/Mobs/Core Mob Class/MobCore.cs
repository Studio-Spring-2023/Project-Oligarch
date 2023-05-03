using System.Collections.Generic;
using System.Xml.Schema;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

/// <summary>
/// The core class for any mobs that exist within the world. Includes GOAP Agent functionality.
/// </summary>
public abstract class MobCore : Core, IGoap
{
	[Range(1f, 25f)]
	public int Damage;

	[Range(1f, 25f)]
	public float MaxMovementOffset;
	[Range(1f, 25f)]
	public float MinMovementOffset;
	public int Action;

	protected NavMeshAgent EntityNavAgent;
	protected NavMeshPath EntityNavPath;

	#region GOAP and FSM Variables
	protected FiniteStateMachine FSM;

	protected FiniteStateMachine.FSMState Idle;
	protected FiniteStateMachine.FSMState MoveTo;
	protected FiniteStateMachine.FSMState PerformAction;

	protected HashSet<KeyValuePair<string, object>> currentGoalPool;
	protected HashSet<GoapAction> availableEntityActionPool;
	protected Stack<GoapAction> currentEntityActions;

	//I don't want this to be instance based because it doesn't really make sense,
	//I may try to experiment with multithreading and make a static planner class
	//that runs on a different thread for performance
	protected GoapPlanner planner;
	#endregion

	#region Awake Function
	protected virtual void Awake()
	{
		//Retrieve the NavAgent on our entity.
		EntityNavAgent = GetComponent<NavMeshAgent>();

		//Generate an empty nav path for use later.
		EntityNavPath = new NavMeshPath();

		//We don't want the nav agent to update the position since
		//it has all sorts of drag and nasty stuff 
		EntityNavAgent.updateRotation = false;

		//Create a new instance of a state machine for our entity
		FSM = new FiniteStateMachine();

		//Create a new instance of the GOAP planner
		planner = new GoapPlanner();

		//Assigns the actions that we wish the entity to have.
		GenerateAvailableEntityActions();

		//The stack of actions that the entity will get from the planner
		currentEntityActions = new Stack<GoapAction>();

		//Create our goal pool for our entity.
		currentGoalPool = new HashSet<KeyValuePair<string, object>>();
		GenerateStartingGoalState();

		//Assigns the state delegates to their defaults.
		CreateIdleState();
		CreateMoveToState();
		CreatePerformActionState();

		//We begin at the idle state, which is when the AI will update themselves with a new plan
		//based on the world state and contextual variables
		FSM.PushState(Idle);
	}
	#endregion

	private void Update()
	{
		//Calls the FSM to update, which is calling our "Idle," "MoveTo," or "PerformAction" state. 
		//These states continuously trigger until their condition is set to true, which is dictated by
		//the specific action that is being triggered.
		FSM.Update(this);
	}

	#region Create FSM States
	private void CreateIdleState()
	{
		Idle = (FSM, entity) =>
		{
			Stack<GoapAction> plan = planner.BuildPlan(this, 
				availableEntityActionPool, 
				MobManager.GetMatchingWorldProperties(currentGoalPool), 
				currentGoalPool);

			if (plan != null)
			{
				currentEntityActions = plan;
				PlanFound(currentGoalPool, currentEntityActions);
				FSM.PushState(PerformAction);
			}
			else
			{
				PlanFailed(currentGoalPool);
			}
		};
	}

	private void CreateMoveToState()
	{
		MoveTo = (FSM, entity) =>
		{
			GoapAction actionToAttempt = currentEntityActions.Peek();

			//First check to make sure the target of the action isn't null, because
			//if it is, we got a problem.
			if (actionToAttempt.TargetObject == null)
			{
				Debug.Log($"<color=red>[StageOneHumanoid] on {entity}: Action target is null. Entity requires target for MoveTo." +
					$"Did you forget to add a target to the action?");
				FSM.PopState(); //Pops out the MoveTo state.
				FSM.PopState(); //Pops out the PerformAction state.
				FSM.PushState(Idle);
				return;
			}

			//MoveSelf returns true once we are within proximity based on the actions requirements.
			if (MoveSelf(actionToAttempt))
				FSM.PopState();
		};
	}

	private void CreatePerformActionState()
	{
		PerformAction = (FSM, entity) =>
		{
			GoapAction actionToAttempt = currentEntityActions.Peek();

			//Check if we finished our action, then remove it if we did.
			if (actionToAttempt.isDone())
				currentEntityActions.Pop();

			//Now check if we don't have any actions in our plan, which
			//means we should return to Idle and get ourselves a new plan.
			//This situation can occur if the entity finishes their last action in the 
			//previous run of this function.
			if (!hasPlan())
			{
				Debug.Log($"<color=orange>[MobCore] on {entity}</color>: " +
					$"No actions left to perform, getting a new plan.");
				FSM.PopState();
				FSM.PushState(Idle);
				return;
			}

			//If we reach this point, we still have an action plan. So let's check to see
			//If we are in range to perform our action. Also need to re-peek at the currentEntityActions
			//In case we did finish a previous action and ensure we have the most up-to-date one.
			actionToAttempt = currentEntityActions.Peek();
			bool inProximity = !actionToAttempt.MustBeInProximity() || actionToAttempt.IsInProximity();

			if (inProximity)
			{
				bool success = actionToAttempt.PerformAction(entity);

				//We didn't succeed, so that means something's wrong with that action.
				//This could mean the world changed between the entity moving to this action or 
				//something else happened while they were working on it.
				if (!success)
				{
					FSM.PopState();
					FSM.PushState(Idle);
					PlanAborted(actionToAttempt);
				}
			}
			else
			{
				//We aren't in range, so we need to get over to where this action is at.
				FSM.PushState(MoveTo);
			}
		};
	}
	#endregion

	#region Abstract Functions
	public abstract bool MoveSelf(GoapAction nextAction);

	protected abstract void GenerateAvailableEntityActions();

	protected abstract void GenerateStartingGoalState();
	#endregion

	#region Helper and Debugging Functions
	protected bool hasPlan() => (currentEntityActions.Count > 0);

	protected void AddActionToPool(GoapAction actionToAdd)
	{
		availableEntityActionPool.Add(actionToAdd);
	}

	protected void RemoveActionFromPool(GoapAction actionToRemove)
	{
		availableEntityActionPool.Remove(actionToRemove);
	}

	protected bool AddGoal(KeyValuePair<string, object> goalToAdd)
	{
		foreach (KeyValuePair<string, object> goal in currentGoalPool)
		{
			//we already have this goal in our current pool, don't add it
			if (goal.Equals(goalToAdd))
			{
				Debug.Log("<color=yellow>[MobCore]</color>: Goal already in Goal pool, wasn't added.");
				return false;
			}
		}

		//Goal was not already in our pool, so add it.
		currentGoalPool.Add(goalToAdd);

		return true;
	}

	protected void RemoveGoal(KeyValuePair<string, object> goalToRemove)
	{
		KeyValuePair<string, object> toRemove = default;
		foreach (KeyValuePair<string, object> goal in currentGoalPool)
		{
			//Found the goal to remove.
			if (goal.Key.Equals(goalToRemove.Key))
				toRemove = goal;
		}

		if (!toRemove.Equals(default))
			currentGoalPool.Remove(toRemove);
		else
			Debug.Log("<color=yellow>[MobCore]</color>: Goal wasn't found in pool, didn't remove.");
	}

	public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
	{
		string plan = "";
		foreach (KeyValuePair<string, object> goal in failedGoal)
		{
			plan += goal.Key + ": " + goal.Value;
			plan += "; ";
		}
		Debug.Log($"<color=red>[MobCore]</color>: Failed to create plan following goal: {plan}");
	}

	public void PlanAborted(GoapAction actionThatAborted)
	{
		Debug.Log($"<color=red>[MobCore]</color>: {actionThatAborted} aborted created plan.");
	}

	public void PlanFound(HashSet<KeyValuePair<string, object>> pursuedGoal, Stack<GoapAction> planOfAction)
	{
		string plan = "";
		foreach (GoapAction action in planOfAction)
		{
			plan += action.GetType().Name;
			plan += "; ";
		}	

		Debug.Log($"<color=green>[MobCore]</color>: Found Plan: {plan}");
	}

	protected void PrintActionPool(GameObject entity)
	{
		string actions = "";
		foreach (GoapAction action in availableEntityActionPool)
		{
			actions += action.GetType().Name;
			actions += "; ";
		}

		Debug.Log($"<color=yellow>[MobCore]</color> on {entity}: Current Action Pool: {actions}");
	}

	protected void PrintGoalPool(GameObject entity)
	{
		string goals = "";
		foreach (KeyValuePair<string, object> goal in currentGoalPool)
		{
			goals += goal.Key + " - " + goal.Value;
			goals += "; ";
		}

		Debug.Log($"<color=yellow>[MobCore]</color> on {entity}: Current Goal Pool: {goals}");
	}
	#endregion
}
