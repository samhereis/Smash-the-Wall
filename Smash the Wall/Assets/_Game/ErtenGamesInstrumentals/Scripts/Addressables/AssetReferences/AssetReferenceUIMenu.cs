#if AddressablesInstalled

using System;
using UI.Canvases;
using UnityEngine.AddressableAssets;

namespace AssetReferences
{
    [Serializable]
    public class AssetReferenceUIMenu : AssetReferenceT<CanvasWindowBase>
    {
        public AssetReferenceUIMenu(string guid) : base(guid)
        {

        }
    }
}

#endif