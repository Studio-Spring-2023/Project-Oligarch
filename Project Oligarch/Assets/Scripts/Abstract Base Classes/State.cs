public abstract class State
{
	protected FiniteStateMachine FSM;
	protected Core EntityCore;

	public State(FiniteStateMachine FSM, Core EntityCore)
	{
		this.EntityCore = EntityCore;
		this.FSM = FSM;
	}

	public virtual void OnStateEnter() { }

	public virtual void Update() { }

	public virtual void FixedUpdate() { }

	public virtual void OnStateExit() { }
}
