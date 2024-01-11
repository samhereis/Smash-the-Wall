using SO;
using System.Collections.Generic;

namespace GOAP.GoapDataClasses
{
    [System.Serializable]
    public class GoapState
    {
        public GOAP_String_SO key;
        public int value;
    }

    public class GoapStates
    {
        public Dictionary<GOAP_String_SO, int> goapStates = new Dictionary<GOAP_String_SO, int>();

        public bool HasState(GOAP_String_SO key)
        {
            return goapStates.ContainsKey(key);
        }

        private void AddState(GOAP_String_SO key, int value)
        {
            goapStates.Add(key, value);
        }

        public void ModifyState(GOAP_String_SO key, int value)
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

        public void RemoveState(GOAP_String_SO key)
        {
            if (HasState(key))
            {
                goapStates.Remove(key);
            }
        }

        public void SetState(GOAP_String_SO key, int value)
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

        public Dictionary<GOAP_String_SO, int> GetStates()
        {
            return goapStates;
        }
    }
}