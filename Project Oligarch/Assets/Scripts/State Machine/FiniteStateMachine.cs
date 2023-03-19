using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public List<State> States = new List<State>();

    State currentState;

    public State GetCurrentState() => currentState;

	public void CurrentStateUpdate() => currentState.Update();

	public void CurrentStateFixedUpdate() => currentState.FixedUpdate();

    public State FindState(int stateIndex)
    {
        return States[stateIndex];
    }

    public void AddState(State stateToAdd)
    {
        States.Add(stateToAdd);
    }

	public void SwitchState(State newState)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = newState;

        currentState.OnStateEnter();
    }
}
