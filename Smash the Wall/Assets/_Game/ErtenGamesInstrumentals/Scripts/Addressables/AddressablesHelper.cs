#if AddressablesInstalled

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Helpers
{
    public class AddressablesHelper
    {
        public static async Awaitable<T> GetAssetAsync<T>(string name)
        {
            var handle = Addressables.LoadAssetAsync<T>(name);
            await handle.Task;

            return handle.Result;
        }

        public static async Awaitable<T> GetAssetAsync<T>(AssetReference assetReference)
        {
            if (assetReference == null)
            {
                Debug.LogWarning($"Adrressable Reference is null: {assetReference.ToString()}");
                return default;
            }

            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            await handle.Task;

            return handle.Result;
        }

        public static async Awaitable<T> InstantiateAsync<T>(string name, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(name, position, rotation, parent);
            await handle.Task;

            if (handle.Result == null)
            {
                return default;
            }

            return handle.Result.GetComponent<T>();
        }

        public static async Awaitable<T> InstantiateAsync<T>(AssetReference assetReference, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) where T : Object
        {
            if (assetReference == null)
            {
                Debug.LogWarning($"Adrressable Reference is null: {assetReference.ToString()}");
                return default;
            }

            var handle = Addressables.InstantiateAsync(assetReference, position, rotation, parent);
            await handle.Task;

            if (handle.Result == null)
            {
                return default;
            }

            return handle.Result.GetComponent<T>();
        }

        public static void Release<T>(T toRelease)
        {
            Addressables.Release<T>(toRelease);
        }

        public static void DestroyObject(GameObject gameObject)
        {
            if (Addressables.ReleaseInstance(gameObject) == false)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}

#endif