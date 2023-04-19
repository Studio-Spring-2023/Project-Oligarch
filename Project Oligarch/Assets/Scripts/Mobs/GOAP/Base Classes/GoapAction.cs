using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Pipeline;
using UnityEngine;

public abstract class GoapAction
{
	protected LayerMask PlayerMask;

	protected HashSet<KeyValuePair<string, object>> preconditions;
    protected HashSet<KeyValuePair<string, object>> effects;

	public float ActionCost;

    public GameObject TargetObject { get; protected set; }
	public float TargetPositionOffset { get; protected set; }

	protected bool inProximity;

    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();

		PlayerMask = LayerMask.GetMask("Player");
	}

    public void ResetAction()
	{
		inProximity = false;

		OnReset();
	}

	protected abstract void OnReset();

    public abstract bool PerformAction(MobCore entity);

    //Contextual preconditions are where we can check our surroundings on a per-action basis
    //to see wether it is reasonable for our entity to try to even attempt that action
    //in the first place, like is something within range or is the player within range?
    public abstract bool CheckProceduralPrecondition(MobCore entity);

	#region Precondition, Effect, and Helper Functions
	protected void AddPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }
    
    protected void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> toRemove = default;
        foreach (KeyValuePair<string, object> pair in preconditions)
        {
            if (pair.Key.Equals(key))
            {
				toRemove = pair;
                break;
			}
		}

        if (!toRemove.Equals(default(KeyValuePair<string, object>)))
            preconditions.Remove(toRemove);
	}

    protected void AddEffect(string key, object value)
    {
		effects.Add(new KeyValuePair<string, object>(key, value));
	}

	protected void RemoveEffect(string key)
	{
		KeyValuePair<string, object> toRemove = default;
		foreach (KeyValuePair<string, object> pair in effects)
		{
			if (pair.Key.Equals(key))
			{
				toRemove = pair;
				break;
			}
		}

		if (!toRemove.Equals(default(KeyValuePair<string, object>)))
			effects.Remove(toRemove);
	}

	public abstract bool isDone();

	public abstract bool MustBeInProximity();

    public bool IsInProximity() => inProximity;

	public void SetInProximity(bool inRange) =>	this.inProximity = inRange;

	public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get { return preconditions; }
    }

	public HashSet<KeyValuePair<string, object>> Effects
	{
		get { return effects; }
	}
	#endregion
}
