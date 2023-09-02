using DataClasses;
using DI;
using Helpers;
using InGameStrings;
using Interfaces;
using Managers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tools
{
    [CreateAssetMenu(fileName = "Scene Loader", menuName = "Scriptables/Helpers/Scene Loader")]
    public class SceneLoader : ScriptableObject, IInitializable, IDIDependent
    {
        public readonly UnityEvent<AScene> onSceneStartLoading = new UnityEvent<AScene>();

        [SerializeField] private bool _loading = false;

        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;

        public void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public async Task LoadSceneAsync(AScene aScene, Action<float> onUpdate = null)
        {
            if (_gameSaveManager == null)
            {
                (this as IDIDependent).LoadDependencies();
            }

            _gameSaveManager.SaveAll();

            if (_loading == false)
            {
                _loading = true;

                onSceneStartLoading?.Invoke(aScene);

                var asyncOperation = SceneManager.LoadSceneAsync(aScene.GetSceneName(), LoadSceneMode.Single);

                while (asyncOperation.isDone == false)
                {
                    onUpdate?.Invoke(asyncOperation.progress);
                    await AsyncHelper.Delay();

                    if (asyncOperation.progress >= 0.9f)
                    {
                        Time.timeScale = 1;
                    }
                }

                _loading = false;
            }
        }

        public void LoadSceneAdditively(int sceneId)
        {
            SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        }
    }
}