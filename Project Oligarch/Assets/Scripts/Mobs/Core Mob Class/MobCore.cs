using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

/// <summary>
/// The core class for any mobs that exist within the world. Includes GOAP Agent functionality.
/// </summary>
public abstract class MobCore : Core, IGoap
{
	protected Mesh EntityMesh;

    protected NavMeshAgent EntityNavAgent;

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

	protected virtual void Awake()
	{
		//Create a new instance of a state machine for our entity
		FSM = new FiniteStateMachine();

		//Create a new instance of the GOAP planner
		planner = new GoapPlanner();

		//this will assign the available actions to this entity
		GenerateAvailableEntityActions();

		currentEntityActions = new Stack<GoapAction>();

		//Create our action pool for our entity
		currentGoalPool = new HashSet<KeyValuePair<string, object>>();
		GenerateStartingGoalState();

		//Assigns the delegate to custom state classes depending on the mob
		CreateIdleState();
		CreateMoveToState();
		CreatePerformActionState();

		//We begin at the idle state, which is when the AI will update themselves with a new plan
		//based on the world state and contextual variables
		FSM.PushState(Idle);
	}

	private void Update()
	{
		//Calls the FSM to update, which is calling our "Idle," "MoveTo," or "PerformAction" state. 
		//These states continuously trigger until their condition is set to true, which is dictated by
		//the specific action that is being triggered.
		FSM.Update(gameObject);
	}

	protected abstract void GenerateAvailableEntityActions();

	protected abstract void CreateIdleState();

	protected abstract void CreateMoveToState();

	protected abstract void CreatePerformActionState();

	public abstract bool MoveSelf(GoapAction nextAction);

	protected abstract void GenerateStartingGoalState();

	#region Helper and Debugging Functions
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
			plan += ", ";
		}
		Debug.Log($"<color=red>[MobCore]</color>: Failed to create plan following goal: ");
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
			plan += ", ";
		}	

		Debug.Log($"<color=green>[MobCore]</color>: Found Plan {plan}.");
	}

	public void PrintActionPool(GameObject entity)
	{
		string actions = "";
		foreach (GoapAction action in availableEntityActionPool)
		{
			actions += action.GetType().Name;
			actions += ", ";
		}

		Debug.Log($"[MobCore] on {entity}: Current Action Pool: {actions}.");
	}

	public void PrintGoalPool(GameObject entity)
	{
		string goals = "";
		foreach (KeyValuePair<string, object> goal in currentGoalPool)
		{
			goals += goal.Key + ": " + goal.Value;
			goals += ", ";
		}

		Debug.Log($"[MobCore] on {entity}: Current Goal Pool: {goals}.");
	}
	#endregion
}
