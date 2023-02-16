using UnityEngine;

public class PlayerAbilityState : State
{
	public PlayerAbilityState(FiniteStateMachine FSM, Core EntityCore) : base(FSM, EntityCore) { }

	public override void OnStateEnter()
	{
		Debug.Log("Ability State Enter");

		//Hook into input functions as we need to know when buttons are pressed

		switch (InputHandler.LastAbilityInput)
		{
			case AbilityInput.Primary:
				PlayerCore.AssignedLoadout.Primary();
				break;

			case AbilityInput.Secondary:
				PlayerCore.AssignedLoadout.Secondary();
				break;

			case AbilityInput.Special:
				PlayerCore.AssignedLoadout.Special();
				break;

			case AbilityInput.Ultimate:
				PlayerCore.AssignedLoadout.Ultimate();
				break;
		}

		//Do input checks to see if the Player is doing something else and switch to that state?
		FSM.SwitchState(FSM.FindState((int)PlayerStates.Idle));
	}

	public override void Update()
	{
		//Debug.Log("Ability State Update");
	}

	public override void FixedUpdate()
	{
		//Debug.Log("Ability State Fixed Update");
	}

	public override void OnStateExit()
	{
		Debug.Log("Ability State Exit");
	}
}
