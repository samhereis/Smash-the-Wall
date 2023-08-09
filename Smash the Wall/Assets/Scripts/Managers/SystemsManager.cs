using ECS.Systems;
using ECS.Systems.CollisionUpdators;
using ECS.Systems.GameState;
using ECS.Systems.Spawners;
using Helpers;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Managers
{
    public class SystemsManager : MonoBehaviour
    {
        [SerializeField] private List<IEnableableSystem> _currentSystems = new List<IEnableableSystem>();

        private CancellationTokenSource _onDestroyCancellationTokenSource = new CancellationTokenSource();

        private async void Awake()
        {
            await AsyncHelper.Delay(2000);
            if (_onDestroyCancellationTokenSource.IsCancellationRequested) return;

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
            _onDestroyCancellationTokenSource.Cancel();
        }

        public void AddSystem(IEnableableSystem system, bool autoEnable)
        {
            _currentSystems.Add(system);

            if (autoEnable) system.Enable();
        }

        public void RemoveSystem(IEnableableSystem system, bool autoDisable)
        {
            _currentSystems.Remove(system);

            if (autoDisable) system.Disable();
        }

        public void TryEnableSystems()
        {
            foreach (var system in _currentSystems)
            {
                if (_onDestroyCancellationTokenSource.IsCancellationRequested) return;

                system.Enable();
            }
        }

        public void TryDisableSystems()
        {
            foreach (var system in _currentSystems)
            {
                if (_onDestroyCancellationTokenSource.IsCancellationRequested) return;

                system?.Disable();
            }
        }
    }
}