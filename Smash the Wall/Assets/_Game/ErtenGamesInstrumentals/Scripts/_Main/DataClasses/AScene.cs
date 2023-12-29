using Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace DataClasses
{
    [Serializable]
    public class AScene : IInitializable
    {
        [ShowInInspector] public string sceneCode { get; private set; }

#if UNITY_EDITOR

        [Required]
        [SerializeField] private SceneAsset _scene;

#endif

        public void Initialize()
        {
#if UNITY_EDITOR

            sceneCode = _scene.name;

#endif
        }
    }
}