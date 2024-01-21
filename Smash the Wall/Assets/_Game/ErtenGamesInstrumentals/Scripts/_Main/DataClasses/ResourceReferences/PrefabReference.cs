using Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ErtenGamesInstrumentals.DataClasses
{
    [Serializable]
    public class PrefabReference<TGetAsComponent> where TGetAsComponent : Component
    {
        public TGetAsComponent target;

        public string targetName => target.name;
        public string targetTypeName => target.GetType().Name;

        public virtual async Task<TGetAsComponent> GetAssetAsync()
        {
            await AsyncHelper.Skip();

            return target.GetComponent<TGetAsComponent>();
        }

        public virtual async Task<T> GetAssetComponentAsync<T>()
        {
            var result = await GetAssetAsync();

            return result.GetComponent<T>();
        }

        public virtual TGetAsComponent GetAsset()
        {
            return target.GetComponent<TGetAsComponent>();
        }

        public virtual T GetAssetComponent<T>()
        {
            var result = GetAsset();

            return result.GetComponent<T>();
        }

        public virtual void Setup()
        {

        }
    }
}