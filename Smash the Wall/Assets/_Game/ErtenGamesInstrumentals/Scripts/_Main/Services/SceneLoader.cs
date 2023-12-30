using DataClasses;
using DependencyInjection;
using Helpers;
using System;
using System.Threading.Tasks;
using UI.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Servies
{
    public class SceneLoader : IDIDependent
    {
        public readonly UnityEvent<AScene> onSceneStartLoading = new UnityEvent<AScene>();

        [SerializeField] private bool _loading = false;

        public virtual async
#if UNITY_2023_2_OR_NEWER
            Awaitable
#else
            Task
#endif
            LoadSceneAsync(AScene aScene, Action<float> onUpdate = null)
        {
            if (_loading == false)
            {
                _loading = true;

                onSceneStartLoading?.Invoke(aScene);

                var asyncOperation = SceneManager.LoadSceneAsync(aScene.sceneCode, LoadSceneMode.Single);

                while (asyncOperation.isDone == false)
                {
                    onUpdate?.Invoke(asyncOperation.progress);
                    await AsyncHelper.NextFrame();

                    if (asyncOperation.progress >= 0.9f)
                    {
                        Time.timeScale = 1;
                    }
                }

                _loading = false;
            }
        }

        public virtual async
#if UNITY_2023_2_OR_NEWER
            Awaitable
#else
            Task
#endif
             LoadSceneAsync(AScene scene, LoadingMenu loadingMenu, Action<float> onUpdate = null)
        {
            if (loadingMenu != null)
            {
                loadingMenu.SetProgress(0f);
                loadingMenu.Enable();

                await AsyncHelper.DelayFloat(1f);

                await LoadSceneAsync(scene, (percent) =>
                {
                    onUpdate?.Invoke(percent);
                    loadingMenu.SetProgress(percent);
                });
            }
            else
            {
                await AsyncHelper.FromAsyncOperation(SceneManager.LoadSceneAsync(scene.sceneCode));
            }
        }

        public virtual void LoadSceneAdditively(int sceneId)
        {
            SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        }
    }
}