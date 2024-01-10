using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SystemsManager : IDisposable
    {
        [SerializeField] private List<IEnableableSystem> _currentSystems = new();

        public SystemsManager(List<IEnableableSystem> enableableSystems)
        {
            _currentSystems.AddRange(enableableSystems);

            TryEnableSystems();
        }

        public void Dispose()
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