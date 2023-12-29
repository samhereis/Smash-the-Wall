using SO;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class UpdateWorld : MonoBehaviour
    {
        [TextArea] public string states;

        private void LateUpdate()
        {
            Dictionary<AString_SO, int> worldStates = GOAPWorld.worldStates.GetStates();
            states = "";

            foreach (KeyValuePair<AString_SO, int> s in worldStates)
            {
                states += s.Key + ", " + s.Value + "\n";
            }
        }
    }
}