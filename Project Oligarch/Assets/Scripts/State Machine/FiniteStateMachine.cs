using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private Stack<FSMState> stateStack = new Stack<FSMState>();

    public delegate void FSMState(FiniteStateMachine FSM, GameObject entity);

    public void Update(GameObject entity)
    {
        if (stateStack.Peek() != null)
            stateStack.Peek().Invoke(this, entity);
        else
            Debug.Log("<color=red>[FiniteStateMachine]</color>: State Stack is empty.");
    }

    public void PopState()
    {
        stateStack.Pop();
    }

    public void PushState(FSMState stateToAdd)
    {
        stateStack.Push(stateToAdd);
    }
}
