using UnityEditor;
using UnityEngine;

namespace Helpers
{
    public static class MonobehaviorHelper
    {
        public static void TrySetDirty(this Object monoBehaviour)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(monoBehaviour);
#endif
        }

        public static void TrySetDirty(this MonoBehaviour monoBehaviour, MonoBehaviour anotherMonoBehaviour)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(anotherMonoBehaviour);
#endif
        }

        public static void DeleteAllMissingScripts(GameObject gameObject)
        {
#if UNITY_EDITOR

            Debug.Log("Deleted missing scripts: " + GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject));
#endif
        }
    }
}