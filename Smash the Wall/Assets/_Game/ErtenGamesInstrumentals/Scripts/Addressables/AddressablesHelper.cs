using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Helpers
{
    public class AddressablesHelper
    {
        public static async Task<T> GetAssetAsync<T>(string name)
        {
#if AddressablesInstalled
            var handle = Addressables.LoadAssetAsync<T>(name);
            await handle.Task;

            return handle.Result;
#else
            await AsyncHelper.Skip();
            return default(T);
#endif
        }

        public static async Task<T> GetAssetAsync<T>(AssetReference assetReference)
        {
#if AddressablesInstalled
            if (assetReference == null)
            {
                Debug.LogWarning($"Adrressable Reference is null: {assetReference.ToString()}");
                return default;
            }

            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            await handle.Task;

            return handle.Result;
#else
            await AsyncHelper.Skip();
            return default(T);
#endif
        }

        public static async Task<T> InstantiateAsync<T>(string name, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null)
        {
#if AddressablesInstalled
            var handle = Addressables.InstantiateAsync(name, position, rotation, parent);
            await handle.Task;

            if (handle.Result == null)
            {
                return default;
            }

            return handle.Result.GetComponent<T>();
#else
            await AsyncHelper.Skip();
            return default(T);
#endif
        }

        public static async Task<T> InstantiateAsync<T>(AssetReference assetReference, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) where T : Object
        {
#if AddressablesInstalled
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
#else
            await AsyncHelper.Skip();
            return default(T);
#endif
        }

        public static void Release<T>(T toRelease)
        {
#if AddressablesInstalled
            Addressables.Release<T>(toRelease);
#endif
        }

        public static void DestroyObject(GameObject gameObject)
        {
#if AddressablesInstalled
            if (Addressables.ReleaseInstance(gameObject) == false)
            {
                Object.Destroy(gameObject);
            }
#endif
        }
    }
}