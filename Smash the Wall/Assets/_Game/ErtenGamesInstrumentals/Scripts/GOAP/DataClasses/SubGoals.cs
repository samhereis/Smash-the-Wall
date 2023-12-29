using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class SubGoals
    {
        public Dictionary<AString_SO, int> subGoals;
        public bool remove;

        public SubGoals(Dictionary<AString_SO, int> goal, bool r)
        {
            subGoals = goal;
            remove = r;
        }
    }
}