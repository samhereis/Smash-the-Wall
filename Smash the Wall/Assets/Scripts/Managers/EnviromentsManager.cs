using Helpers;
using Identifiers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public sealed class EnviromentsManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private AssetReferenceGameObject[] _suitableEnviroments;

        [Header("Debug")]
        [SerializeField] private EnviromentIdentifier _enviroment;

        private async void Awake()
        {
            AssetReferenceGameObject enviroment = _suitableEnviroments[Random.Range(0, _suitableEnviroments.Length)];

            _enviroment = await AddressablesHelper.InstantiateAsync<EnviromentIdentifier>(enviroment);

            if (enviroment != null)
            {
                DontDestroyOnLoad(_enviroment);
            }
        }

        private void OnDestroy()
        {
            AddressablesHelper.DestroyObject(_enviroment.gameObject);
        }
    }
}