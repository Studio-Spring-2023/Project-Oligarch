using System.Collections.Generic;

public interface IGoap
{
    bool MoveSelf(GoapAction nextAction);

	#region Debugging Functionality
	void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

	void PlanAborted(GoapAction actionThatAborted);

	void PlanFound(HashSet<KeyValuePair<string, object>> pursuedGoal, Stack<GoapAction> planOfAction);
	#endregion
}
