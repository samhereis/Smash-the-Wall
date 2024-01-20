using System;

#if AddressablesInstalled
using UnityEngine.AddressableAssets;
using UnityEngine;
#endif

namespace AssetReferences
{
    [Serializable]
    public class AssetReferenceMaterial

#if AddressablesInstalled
                                        : AssetReferenceT<Material>
#endif
    {
        public AssetReferenceMaterial(string guid)
#if AddressablesInstalled
                                                    : base(guid)
#endif
        {

        }
    }
}