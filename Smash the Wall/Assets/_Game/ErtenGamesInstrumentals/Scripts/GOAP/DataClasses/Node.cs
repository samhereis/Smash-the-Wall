using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<AString_SO, int> state;
        public GOAPAction action;

        public Node(Node parent, float cost, Dictionary<AString_SO, int> allStates, GOAPAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new Dictionary<AString_SO, int>(allStates);
            this.action = action;
        }

        public Node(Node parent, float cost, Dictionary<AString_SO, int> allStates, Dictionary<AString_SO, int> beliefStates, GOAPAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new Dictionary<AString_SO, int>(allStates);

            foreach (KeyValuePair<AString_SO, int> b in beliefStates)
            {
                if (!this.state.ContainsKey(b.Key))
                {
                    this.state.Add(b.Key, b.Value);
                }
            }

            this.action = action;
        }
    }
}