using Configs;
using DataClasses;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllScenes", menuName = "Scriptables/Lists/ListOfAllScenes")]
    public class ListOfAllScenes : ConfigBase, IInitializable
    {
        [field: SerializeField] public List<AScene> _scenes { get; private set; } = new List<AScene>();
        [field: SerializeField] public AScene mainMenuScene { get; private set; }
        [field: SerializeField] public int currentSceneIndex { get; private set; }

        public override void Initialize()
        {
            foreach (var scene in _scenes)
            {
                scene.Initialize();
            }
        }

        public AScene GetNextScene()
        {
            currentSceneIndex++;
            return GetCurrentScene();
        }

        public AScene GetCurrentScene()
        {
            if (currentSceneIndex >= _scenes.Count)
            {
                currentSceneIndex = 0;
            }

            return _scenes[currentSceneIndex];
        }

        public AScene GetRandomScene()
        {
            return _scenes[Random.Range(0, _scenes.Count)];
        }
    }
}