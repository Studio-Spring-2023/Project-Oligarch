using UnityEngine;

public class PlayerMoveState : State
{
	public PlayerMoveState(FiniteStateMachine FSM, Core EntityCore) : base(FSM, EntityCore) { }

	public override void OnStateEnter()
	{
		Debug.Log("Move State Enter");
	}

	public override void Update()
	{
		Debug.Log("Move State Update");
	}

	public override void FixedUpdate()
	{
		Debug.Log("Move State Fixed Update");
	}

	public override void OnStateExit()
	{
		Debug.Log("Move State Exit");
	}
}
