using UnityEngine;

public class DeadState : State
{
	public DeadState(FiniteStateMachine FSM, Core EntityCore) : base(FSM, EntityCore) { }

	public override void OnStateEnter()
	{
		Debug.Log("Dead State Enter");
	}

	public override void Update()
	{
		Debug.Log("Dead State Update");
	}

	public override void FixedUpdate()
	{
		Debug.Log("Dead State Fixed Update");
	}

	public override void OnStateExit()
	{
		Debug.Log("Move State Exit");
	}
}
