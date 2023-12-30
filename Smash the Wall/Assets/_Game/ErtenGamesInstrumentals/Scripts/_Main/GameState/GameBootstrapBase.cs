using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameState
{
    public class GameBootstrapBase : MonoBehaviour, IInitializable
    {
        public virtual void Initialize()
        {

        }
    }
}