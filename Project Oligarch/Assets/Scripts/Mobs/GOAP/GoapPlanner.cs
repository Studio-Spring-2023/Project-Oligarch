using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner
{
    //This is the entry point function of the GOAP Planner, and is what all Mobs will call in an attempt
    //to construct a valid action plan.

    public Stack<GoapAction> BuildPlan(GameObject entity, 
        HashSet<GoapAction> entityActionPool, 
        HashSet<KeyValuePair<string, object>> startingWorldState, 
        HashSet<KeyValuePair<string, object>> desiredEntityGoals)
    {
        
        //Will probably need to reset actions, but unsure if this use case applies to our game
        //entity.resetActions

        //First, check all of our available actions, which ones meet our contextual preconditions
        HashSet<GoapAction> useableActions = new HashSet<GoapAction>();
        foreach (GoapAction a in entityActionPool)
        {
            if (a.CheckProceduralPrecondition(entity))
                useableActions.Add(a);
        }
        //Now we have a collection of Actions that we can reasonably perform

        //Now we need to construct a graph based on our desired goal and get a queue of actions back
        //which our entity can then perform. The BuildGraph function returns possible final nodes
        //that take us to our end goal. BuildGraph is recursive, so we have a set of possible nodes that are 'end results,'
        //each with a string of child nodes leading up to them. We can pick from these nodes based on certain criteria, like the cheapest.
        List<Node> possibleFinalActions = new List<Node>();

        Node startingNode = new Node(null, 0, startingWorldState, null);
        bool success = ConstructActionGraph(startingNode, possibleFinalActions, useableActions, desiredEntityGoals);

        //First check to see if we succeeded in constructing a graph.
        if (!success)
        {
            Debug.Log($"<color=yellow>[GOAP Planner] on {entity}</color>: Failed to construct a graph.");
            return null;
        }

        //Since we know that we have a graph, we now need to find the cheapest final node,
        //The cheapest node means we have the set of actions that require the least amount
        //of weight to achieve the end goal.
        Node cheapestFinalNode = null;
        foreach (Node potentialFinalActionNode in possibleFinalActions)
        {
            if (cheapestFinalNode == null)
                cheapestFinalNode = potentialFinalActionNode;
            else
            {
                if (potentialFinalActionNode.runningCost < cheapestFinalNode.runningCost)
                    cheapestFinalNode = potentialFinalActionNode;
            }
        }

        //Now that we've found our cheapest final action, we need to work backwards and
        //construct the order in which our entity can reach the end goal from their start goal.
        //We use a stack, so the last action put on the stack is the first action that the entity
        //should perform.
		Stack<GoapAction> entityActionPlan = new Stack<GoapAction>();
        Node node = cheapestFinalNode;
        while (node != null)
        {
            if (node.assignedAction != null)
                entityActionPlan.Push(node.assignedAction);
            node = node.parent;
        }
        
        //Finally, return the stack to our entity so it can begin performing the actions
        //that the planner has created for it.
		return entityActionPlan;
    }

    private bool ConstructActionGraph(Node parentNode, 
        List<Node> possibleFinalActions, 
        HashSet<GoapAction> usableActions, 
        HashSet<KeyValuePair<string, object>> desiredEntityGoals)
    {
        bool foundPath = false;

        //Begin iterating through the usable actions
        foreach(GoapAction possibleAction in usableActions)
        {
            //We first need to check if the possible actions between the parent state and the action
            //match up. We're checking to see if the preconditions of the action are fulfilled by
            //the current world state, because if they aren't, we have to pursue a different
            //action that does have their conditions met by the current world state.
            if (DoPreconditionsMatch(possibleAction.Preconditions, parentNode.accumluatedWorldState))
            {

                //Since our preconditions do match up between this desired action and the nodes worldstate,
                //then we need to combine the effects of this action with that of the parents node,
                //so that we can update the 'accumulated world state' and further test for more actions
                //to reach our desired goal, or perhaps with this we have reached our desired goal.
                HashSet<KeyValuePair<string, object>> combinedState = ApplyActionEffects(parentNode.accumluatedWorldState, possibleAction.Effects);

                Node newNode = new Node(parentNode, parentNode.runningCost + possibleAction.ActionCost, combinedState, possibleAction);

                //Check if we have reached a state where our entity goals align with our combined state.
                //The combined state was the result of applying an actions effects to our world state
                if (DoPreconditionsMatch(desiredEntityGoals, combinedState))
                {
                    //We have found a node that reaches our desired goal, so we should add it to our 
                    possibleFinalActions.Add(newNode);
                    foundPath = true;
                }
                else
                {
                    //This is where recursion comes in. We haven't find a node yet that satisfies our desired goal state,
                    //so we call this function again, passing it our current node, and repeating the whole process again.
                    //We do this recursion for *every* possible action that our entity has. What will happen is our
                    //actions will each be fully explored (Full Breadth Search) for a possible plan, which isn't exactly ideal,
                    //but since our system is simple, this shouldn't be a performance issue. Implementation a more specific algorithm
                    //can do us good in performance.
                    HashSet<GoapAction> potentialActionSubset = RemoveExhaustedAction(usableActions, possibleAction);

                    foundPath = ConstructActionGraph(newNode, possibleFinalActions, potentialActionSubset, desiredEntityGoals);
				}
			}
        }

        return foundPath;
    }

    private bool DoPreconditionsMatch(HashSet<KeyValuePair<string, object>> potentialActionPreconditions,
        HashSet<KeyValuePair<string, object>> nodeWorldState)
    {
		bool match = false;

		//This translates to: "Do the preconditions of our action match with the nodes world state?"
		foreach (KeyValuePair<string, object> precondition in potentialActionPreconditions)
        {
            
            foreach(KeyValuePair<string, object> worldState in nodeWorldState)
            {
                if (worldState.Equals(precondition))
                {
					match = true;
					break;
				}
            }
        }

        Debug.Log($"<color=yellow>[GOAP Planner]</color>: Preconditions match: {match}");
        return match;
    }

    private HashSet<KeyValuePair<string, object>> ApplyActionEffects(HashSet<KeyValuePair<string, object>> currentWorldState, 
        HashSet<KeyValuePair<string, object>> actionEffects)
    {
        HashSet<KeyValuePair<string, object>> combinedState = new HashSet<KeyValuePair<string, object>>();

        //Copy the values from our current world state to a new hash set so we can remove
        //or update any values that are going to be adjusted by the action effects.
        foreach (KeyValuePair<string, object> state in currentWorldState)
        {
            combinedState.Add(state);
        }

        foreach(KeyValuePair<string, object> effect in actionEffects)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> state in combinedState)
            {
                if (state.Equals(effect))
                {
					match = true;
                    break;
				}
			}

            //If our state in currentWorldState matched with an effect in actionEffects, then we need to update that state 
            //to whatever changes that action effect had on it too. If our currentWorldState didn't have a value that matched with what
            //the effect does, then that means we need to add it to our combinedState as it is a new change to that world state.
            if (match)
            {
                //This is a bit of a weird function that takes in a delegate. The kvp is the parameters to a function, and the
                //=> marks the body of our delegate. Essentialy this delegate is called within RemoveWhere and is passed a KVP from
                //combined state and then runs the code block that we defined, checking to see if any of the kvp's in combinedState
                //match with the effect, effectively removing any duplicates.
                combinedState.RemoveWhere( (KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(effect.Key); } );

                combinedState.Add(effect);
			}
            else
				combinedState.Add(effect);
		}

		return combinedState;
    }

    private HashSet<GoapAction> RemoveExhaustedAction(HashSet<GoapAction> potentialActions, GoapAction exhaustedAction)
    {
        HashSet<GoapAction> potentailSubset = new HashSet<GoapAction>();
        foreach(GoapAction action in potentialActions)
        {
            if (!action.Equals(exhaustedAction))
                potentailSubset.Add(action);
		}

        return potentailSubset;
	}

	//The node class that populates our graph.
	public class Node
    {
        public Node parent;
        public float runningCost;
        //Not sure if I like this name. Trying to find something that's more readable that 
        //intuitvly makes senese when you read it. Accumulated is used here because
        //every node gains the previous one's effects to create a new world state,
        //which if you just called this 'state,' seems a bit ambigious in my opinion
        public HashSet<KeyValuePair<string, object>> accumluatedWorldState;
        public GoapAction assignedAction;

        public Node(Node parentNode, 
            float runningCost, 
            HashSet<KeyValuePair<string, object>> accumluatedWorldState, 
            GoapAction assignedAction)
        {
            parent = parentNode;
            this.runningCost = runningCost;
            this.accumluatedWorldState = accumluatedWorldState;
            this.assignedAction = assignedAction;
        }
    }
}
