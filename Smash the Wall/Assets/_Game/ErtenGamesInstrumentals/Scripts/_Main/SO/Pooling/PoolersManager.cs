using Pooling;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.World.Helpers
{
    public sealed class PoolersManager : MonoBehaviour
    {
        [Required]
        [SerializeField] private List<PoolerBase<MonoBehaviour>> _poolerBases = new List<PoolerBase<MonoBehaviour>>();

        private async void Awake()
        {
            foreach (var pooler in _poolerBases)
            {
                pooler.Clear();
                await pooler.SpawnAsync();
            }
        }

        private void OnDisable()
        {
            foreach (var pooler in _poolerBases)
            {
                pooler.Clear();
            }
        }
    }
}