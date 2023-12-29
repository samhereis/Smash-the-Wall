using DependencyInjection;
using Identifiers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public sealed class EnviromentsManager : MonoBehaviour, IDIDependent
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
            if (_enviroment != null)
            {
                _enviroment.Initialize();
                DontDestroyOnLoad(_enviroment);
            }
        }
    }
}