using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class GOAPPlanner
    {
        public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<GOAP_String_SO, int> goal, GoapStates beliefStates)
        {
            List<GOAPAction> usableActions = new List<GOAPAction>();

            foreach (GOAPAction a in actions)
            {
                if (a.IsAchievable())
                {
                    usableActions.Add(a);
                }
            }

            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0.0f, GOAPWorld.worldStates.GetStates(), beliefStates.GetStates(), null);

            bool success = BuildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                return null;
            }

            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }

            List<GOAPAction> result = new List<GOAPAction>();
            Node n = cheapest;

            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }

                n = n.parent;
            }

            Queue<GOAPAction> queue = new Queue<GOAPAction>();

            foreach (GOAPAction a in result)
            {
                queue.Enqueue(a);
            }

            return queue;
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, Dictionary<GOAP_String_SO, int> goal)
        {
            bool foundPath = false;

            foreach (GOAPAction action in usableActions)
            {
                if (action.IsAhievableGiven(parent.state))
                {
                    Dictionary<GOAP_String_SO, int> currentState = new Dictionary<GOAP_String_SO, int>(parent.state);

                    foreach (KeyValuePair<GOAP_String_SO, int> eff in action.baseSettings.afterEffects)
                    {
                        if (!currentState.ContainsKey(eff.Key))
                        {
                            currentState.Add(eff.Key, eff.Value);
                        }
                    }

                    Node node = new Node(parent, parent.cost + action.baseSettings.cost, currentState, action);

                    if (GoalAchieved(goal, currentState))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        List<GOAPAction> subset = ActionSubset(usableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal);

                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }
            return foundPath;
        }

        private List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction removeMe)
        {
            List<GOAPAction> subset = new List<GOAPAction>();

            foreach (GOAPAction a in actions)
            {
                if (!a.Equals(removeMe))
                {
                    subset.Add(a);
                }
            }

            return subset;
        }

        private bool GoalAchieved(Dictionary<GOAP_String_SO, int> goal, Dictionary<GOAP_String_SO, int> state)
        {
            foreach (KeyValuePair<GOAP_String_SO, int> g in goal)
            {
                if (!state.ContainsKey(g.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}