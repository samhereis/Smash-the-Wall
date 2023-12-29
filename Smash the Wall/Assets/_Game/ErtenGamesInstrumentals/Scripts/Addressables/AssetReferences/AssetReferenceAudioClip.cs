#if AddressablesInstalled

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetReferences
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid)
        {

        }
    }
}

#endif