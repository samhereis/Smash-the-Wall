using Configs;
using DataClasses;
using Helpers;
using Interfaces;
using Managers;
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

            mainMenuScene.Initialize();

            currentSceneIndex = GameSaveManager.GetLevelSave().sceneIndex;

            this.TrySetDirty();
        }

        public void SetNextScene()
        {
            currentSceneIndex++;

            if (currentSceneIndex >= _scenes.Count)
            {
                currentSceneIndex = 0;
            }

            SetScene(currentSceneIndex);
        }

        public AScene GetCurrentScene()
        {
            if (currentSceneIndex >= _scenes.Count)
            {
                currentSceneIndex = 0;
                SetScene(currentSceneIndex);
            }

            return _scenes[currentSceneIndex];
        }

        public AScene GetRandomScene()
        {
            return _scenes[Random.Range(0, _scenes.Count)];
        }

        private void SetScene(int index)
        {
            var levelSave = GameSaveManager.GetLevelSave();
            levelSave.sceneIndex = index;
        }
    }
}