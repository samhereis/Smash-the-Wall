using DependencyInjection;
using Helpers;
using Identifiers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Managers
{
    public sealed class EnviromentsManager : MonoBehaviour, INeedDependencyInjection
    {
        [Header("Addressables")]
        [Required]
        [SerializeField] private EnviromentIdentifier[] _suitableEnviroments;

        [Header("Debug")]
        [SerializeField] private EnviromentIdentifier _enviroment;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_suitableEnviroments.IsNullOrEmpty() == false)
            {
                _enviroment = Instantiate(_suitableEnviroments.GetRandom());
            }

            if (_enviroment != null)
            {
                _enviroment.Initialize();
            }
        }
    }
}