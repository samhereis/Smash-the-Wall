using System;

#if AddressablesInstalled
using UnityEngine.AddressableAssets;
using UnityEngine;
#endif

namespace AssetReferences
{
    [Serializable]
    public class AssetReferenceAudioClip
#if AddressablesInstalled
                                        : AssetReferenceT<AudioClip>
#endif

    {
        public AssetReferenceAudioClip(string guid)
#if AddressablesInstalled
                                                    : base(guid)
#endif
        {

        }
    }
}