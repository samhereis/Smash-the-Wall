using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    [System.Serializable]
    public class GoapState
    {
        public AString_SO key;
        public int value;
    }

    public class GoapStates
    {
        public Dictionary<AString_SO, int> goapStates = new Dictionary<AString_SO, int>();

        public bool HasState(AString_SO key)
        {
            return goapStates.ContainsKey(key);
        }

        private void AddState(AString_SO key, int value)
        {
            goapStates.Add(key, value);
        }

        public void ModifyState(AString_SO key, int value)
        {
            if (HasState(key))
            {
                goapStates[key] += value;

                if (goapStates[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                AddState(key, value);
            }
        }

        public void RemoveState(AString_SO key)
        {
            if (HasState(key))
            {
                goapStates.Remove(key);
            }
        }

        public void SetState(AString_SO key, int value)
        {
            if (HasState(key))
            {
                goapStates[key] = value;
            }
            else
            {
                AddState(key, value);
            }
        }

        public Dictionary<AString_SO, int> GetStates()
        {
            return goapStates;
        }
    }
}