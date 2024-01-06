using ECS.Systems;
using ECS.Systems.CollisionUpdators;
using ECS.Systems.GameState;
using ECS.Systems.Spawners;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SystemsManager : MonoBehaviour
    {
        [SerializeField] private List<IEnableableSystem> _currentSystems = new List<IEnableableSystem>();

        private void Awake()
        {
            _currentSystems.Add(PictureSpawner_System.instance);
            _currentSystems.Add(DestroyableCollisionUpdator_System.instance);
            _currentSystems.Add(ChangeKinematicOnCollided_Updator.instance);
            _currentSystems.Add(CheckPicturePieceKinematic_System.instance);
            _currentSystems.Add(DestroyDestroyables_System.instance);
            _currentSystems.Add(WinLoseChecker_System.instance);

            TryEnableSystems();
        }

        private void OnDestroy()
        {
            TryDisableSystems();
        }

        public void TryEnableSystems()
        {
            foreach (var system in _currentSystems)
            {
                system.Enable();
            }
        }

        public void TryDisableSystems()
        {
            foreach (var system in _currentSystems)
            {
                system?.Disable();
            }
        }
    }
}