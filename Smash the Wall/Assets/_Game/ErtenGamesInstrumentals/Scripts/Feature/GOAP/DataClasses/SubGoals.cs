using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    public class SubGoals
    {
        public Dictionary<GOAP_String_SO, int> subGoals;
        public bool remove;

        public SubGoals(Dictionary<GOAP_String_SO, int> goal, bool r)
        {
            subGoals = goal;
            remove = r;
        }
    }
}