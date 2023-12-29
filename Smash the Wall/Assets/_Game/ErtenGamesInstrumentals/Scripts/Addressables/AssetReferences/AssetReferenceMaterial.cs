#if AddressablesInstalled

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetReferences
{
    [Serializable]
    public class AssetReferenceMaterial : AssetReferenceT<Material>
    {
        public AssetReferenceMaterial(string guid) : base(guid)
        {

        }
    }
}

#endif