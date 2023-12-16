using Interfaces;
using System;
using UnityEditor;
using UnityEngine;

namespace DataClasses
{
    [Serializable]
    public class AScene : IInitializable
    {
        [field: SerializeField] public string sceneName { get; private set; }

#if UNITY_EDITOR

        [SerializeField] private SceneAsset _scene;

#endif

        public void Initialize()
        {
#if UNITY_EDITOR

            sceneName = _scene.name;

#endif
        }

        public string GetSceneName()
        {
            return sceneName;
        }
    }
}