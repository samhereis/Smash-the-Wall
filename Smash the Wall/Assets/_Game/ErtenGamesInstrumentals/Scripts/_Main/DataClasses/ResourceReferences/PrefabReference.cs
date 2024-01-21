using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ErtenGamesInstrumentals.DataClasses
{
    [Serializable]
    public class PrefabReference<TGetAsComponent> where TGetAsComponent : Component
    {
#if UNITY_EDITOR
        [field: Required]
        [field: Sirenix.OdinInspector.FilePath]
        [field: OnValueChanged(nameof(Setup))]
        [field: SerializeField] public string stringReference { get; private set; }
#endif


        [field: FoldoutGroup("Debug")]
        [field: SerializeField, ReadOnly] protected string assetReference { get; private set; }

        [field: FoldoutGroup("Debug")]
        [field: SerializeField, ReadOnly] public string typeReference { get; private set; }

        [field: FoldoutGroup("Debug")]
        [field: SerializeField, ReadOnly] public string objectName { get; private set; }

#if UNITY_EDITOR
        [FoldoutGroup("Debug")]
        [ReadOnly] public TGetAsComponent target;
#endif

        public virtual async Task<TGetAsComponent> GetAssetAsync()
        {
            var request = Resources.LoadAsync<GameObject>(assetReference);
            while (request.isDone == false) { await AsyncHelper.Skip(); }
            var result = request.asset as GameObject;

            return result.GetComponent<TGetAsComponent>();
        }

        public virtual async Task<T> GetAssetComponentAsync<T>()
        {
            var result = await GetAssetAsync();

            return result.GetComponent<T>();
        }

        [FoldoutGroup("Debug"), Button]
        public virtual TGetAsComponent GetAsset()
        {
            var result = Resources.Load<GameObject>(assetReference);

            return result.GetComponent<TGetAsComponent>();
        }

        public virtual T GetAssetComponent<T>()
        {
            var result = GetAsset();

            return result.GetComponent<T>();
        }

        public virtual async void Setup()
        {
#if UNITY_EDITOR

            assetReference = stringReference;

            int index = assetReference.IndexOf("Resources/");
            if (index != -1)
            {
                assetReference = assetReference.Substring(index + "Resources/".Length);
            }

            assetReference = assetReference.Replace(".prefab", "");

            target = await GetAssetAsync();

            if (target != null)
            {
                typeReference = target.GetType().Name;
                objectName = target.name;
            }
#endif
        }
    }
}