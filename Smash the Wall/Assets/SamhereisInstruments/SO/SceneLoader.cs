using Helpers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tools
{
    [CreateAssetMenu(fileName = "Scene Loader", menuName = "Scriptables/Helpers/Scene Loader")]
    public class SceneLoader : ScriptableObject
    {
        public readonly UnityEvent onSceneStartLoading = new UnityEvent();

        private static bool _loading = false;

        public async void LoadScene(int index)
        {
            await StartLoadScene(index);
        }

        private async Task StartLoadScene(int sceneId)
        {
            if (_loading == false)
            {
                _loading = true;
                onSceneStartLoading?.Invoke();

                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
                asyncOperation.allowSceneActivation = false;

                while (asyncOperation.isDone == false)
                {
                    await AsyncHelper.Delay(40);

                    if (asyncOperation.progress == 0.9f)
                    {
                        _loading = false;
                        Time.timeScale = 1;

                        asyncOperation.allowSceneActivation = true;

                        break;
                    }
                }
            }
        }

        public void LoadSceneAdditively(int sceneId)
        {
            SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        }
    }
}
