using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameState
{
    public class GameBootstrapBase : MonoBehaviour, IInitializable
    {
        [Required]
        [ShowInInspector] private IGameStateChanger _gameStateChanger;

        public virtual void Initialize()
        {

        }
    }
}