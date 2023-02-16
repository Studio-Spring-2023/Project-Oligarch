using UnityEngine;

public class PlayerIdleState : State
{
	public string StateName { get; private set; }

	public PlayerIdleState(FiniteStateMachine FSM, PlayerCore EntityCore) : base(FSM, EntityCore) 
	{
		//StateName = stateName;
		//, string stateName
	}

	public override void OnStateEnter()
    {
		Debug.Log("Idle State Enter");

		//Hook into input functions as we need to know when buttons are pressed
		InputHandler.OnAbilityInput += AbilityKeyPressed;
    }

	public override void Update()
	{
		//Debug.Log("Idle State Update");
	}

	public override void FixedUpdate()
	{
		//Debug.Log("Idle State Fixed Update");
	}

	private void AbilityKeyPressed()
	{
		FSM.SwitchState(FSM.FindState( (int)PlayerStates.Ability) );
	}

	public override void OnStateExit()
    {
		InputHandler.OnAbilityInput -= AbilityKeyPressed;
		Debug.Log("Idle State Exit");
	}
}
