using Helpers;
using Interfaces;
using UnityEngine;

namespace Identifiers
{
    public sealed class EnviromentIdentifier : IdentifierBase, IInitializable
    {
        [SerializeField] private AssetReferenceMaterial _skyBox;

        public async void Initialize()
        {
            if(_skyBox == null) return;

            var skyBox = await AddressablesHelper.GetAssetAsync<Material>(_skyBox);

            if (skyBox != null)
            {
                RenderSettings.skybox = skyBox;
            }

        }
    }
}