using DI;
using Helpers;
using Identifiers;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Managers
{
    public sealed class EnviromentsManager : MonoBehaviour, IDIDependent, IInitializable<int>, IClearable
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)] private Input_SO _input;

        [Header("Addressables")]
        [SerializeField] private AssetReferenceGameObject[] _suitableEnviroments;

        [Header("Debug")]
        [SerializeField] private EnviromentIdentifier _enviroment;
        [SerializeField] private int _currentIndex = 0;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            Initialize(_currentIndex);

            _input.input.Player.ChangeWeapon.performed += ChangeEnviroment;
        }

        private void OnDestroy()
        {
            Clear();

            _input.input.Player.ChangeWeapon.performed -= ChangeEnviroment;
        }

        private void ChangeEnviroment(InputAction.CallbackContext context)
        {
            Clear();

            _currentIndex++;

            if (_currentIndex >= _suitableEnviroments.Length)
            {
                _currentIndex = 0;
            }

            Initialize(_currentIndex);
        }

        public async void Initialize(int index)
        {
            AssetReferenceGameObject enviroment = _suitableEnviroments[index];

            _enviroment = await AddressablesHelper.InstantiateAsync<EnviromentIdentifier>(enviroment);

            if (_enviroment != null)
            {
                _enviroment.Initialize();
                DontDestroyOnLoad(_enviroment);
            }
        }

        public void Clear()
        {
            Delete(_enviroment);

            foreach (var enviroment in FindObjectsOfType<EnviromentIdentifier>(true))
            {
                Delete(enviroment);
            }

            void Delete(EnviromentIdentifier enviroment)
            {
                if (enviroment == null) { return; }

                try
                {
                    AddressablesHelper.DestroyObject(enviroment.gameObject);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning("Could not delete enviroment: " + exception.Message);
                }
            }
        }
    }
}