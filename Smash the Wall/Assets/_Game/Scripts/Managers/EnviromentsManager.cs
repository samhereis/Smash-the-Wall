using DependencyInjection;
using ErtenGamesInstrumentals.DataClasses;
using Helpers;
using Identifiers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Managers
{
    public sealed class EnviromentsManager : MonoBehaviour, INeedDependencyInjection, ISelfValidator
    {
        [Header("Addressables")]
        [Required]
#if UNITY_EDITOR
        [ListDrawerSettings(ListElementLabelName = (nameof(PrefabReference<EnviromentIdentifier>.objectName)))]
#endif
        [SerializeField] private PrefabReference<EnviromentIdentifier>[] _suitableEnviroments;

        [Header("Debug")]
        [SerializeField] private EnviromentIdentifier _enviroment;

        public void Validate(SelfValidationResult result)
        {
            foreach (var enviroment in _suitableEnviroments)
            {
                enviroment.Setup();
            }
        }

        private void Start()
        {
            Initialize();
        }

        public async void Initialize()
        {
            if (_suitableEnviroments.IsNullOrEmpty() == false)
            {
                _enviroment = Instantiate(await _suitableEnviroments.GetRandom().GetAssetAsync());
            }

            if (_enviroment != null)
            {
                _enviroment.Initialize();
            }
        }
    }
}