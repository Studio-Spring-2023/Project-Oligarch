using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackPlayer : GoapAction
{
    public MeleeAttackPlayer()
    {
		//Here is where we add preconditions and effects
		preconditions.Add(new KeyValuePair<string, object>("playerAlive", true));
		effects.Add(new KeyValuePair<string, object>("playerAlive", false));
    }

	public override bool CheckProceduralPrecondition(GameObject entity)
	{
		return true;
	}

	public override bool isDone()
	{
		throw new System.NotImplementedException();
	}

	public override bool MustBeInProximity()
	{
		throw new System.NotImplementedException();
	}

	public override void PerformAction(GameObject entity)
	{
		throw new System.NotImplementedException();
	}

	public override void ResetAction()
	{
		throw new System.NotImplementedException();
	}
}

public class RangeAttackPlayer : GoapAction
{
	public RangeAttackPlayer()
	{
		//Here is where we add preconditions and effects
	}

	public override bool CheckProceduralPrecondition(GameObject entity)
	{
		throw new System.NotImplementedException();
	}

	public override bool isDone()
	{
		throw new System.NotImplementedException();
	}

	public override bool MustBeInProximity()
	{
		throw new System.NotImplementedException();
	}

	public override void PerformAction(GameObject entity)
	{
		throw new System.NotImplementedException();
	}

	public override void ResetAction()
	{
		throw new System.NotImplementedException();
	}
}
