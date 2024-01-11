using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ErtenGamesInstrumentals.DataClasses
{
    [Serializable]
    public class PrefabReference<TGetAsComponent> where TGetAsComponent : Component
    {
        [field: FoldoutGroup("Debug")]
        [field: SerializeField, ReadOnly] public string stringReference { get; private set; }

        [field: FoldoutGroup("Debug")]
        [field: SerializeField, ReadOnly] public string typeReference { get; private set; }

#if UNITY_EDITOR

        [FoldoutGroup("Debug")]
        [SerializeField] private string[] _removeFromStringReference = { ".prefab" };

        [Required]
        [OnValueChanged(nameof(Setup))]
        [SerializeField] protected TGetAsComponent _resourceReference;

        public PrefabReference(TGetAsComponent resourceReference)
        {
            _resourceReference = resourceReference;
        }

        private string _objectName = string.Empty;
        public string objectName
        {
            get
            {
                if (string.IsNullOrEmpty(_objectName) == true) { SetObjectName(); };
                return _objectName;
            }
        }

#endif

        public virtual async Task<TGetAsComponent> GetAssetAsync()
        {
            var request = Resources.LoadAsync<GameObject>(stringReference);
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
            var result = Resources.Load<GameObject>(stringReference);

            return result.GetComponent<TGetAsComponent>();
        }

        public virtual T GetAssetComponent<T>()
        {
            var result = GetAsset();

            return result.GetComponent<T>();
        }

        [FoldoutGroup("Debug"), Button]
        public virtual void Setup()
        {
#if UNITY_EDITOR

            if (_resourceReference != null)
            {
                stringReference = AssetDatabase.GetAssetPath(_resourceReference);

                int index = stringReference.IndexOf("Resources/");
                if (index != -1)
                {
                    stringReference = stringReference.Substring(index + "Resources/".Length);
                }

                foreach (var removeFromStringReference in _removeFromStringReference)
                {
                    stringReference = stringReference.Replace(removeFromStringReference, "");
                }

                typeReference = _resourceReference.GetType().Name;

                SetObjectName();
            }

#endif
        }

        private void SetObjectName()
        {
#if UNITY_EDITOR

            if (_resourceReference == null) { return; }

            _objectName = _resourceReference.name;
#endif
        }
    }
}