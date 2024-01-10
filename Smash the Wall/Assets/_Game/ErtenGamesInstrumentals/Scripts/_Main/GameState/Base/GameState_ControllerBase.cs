using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public abstract class GameState_ControllerBase
    {
        protected async
#if UNITY_2023_1_OR_NEWER
            Awaitable
#else
             System.Threading.Tasks.Task
#endif
        LoadScene(string sceneName)
        {
            var handler = SceneManager.LoadSceneAsync(sceneName);
            while (handler.isDone == false) { await AsyncHelper.Skip(); }
        }

        protected async
#if UNITY_2023_1_OR_NEWER
            Awaitable
#else
             System.Threading.Tasks.Task
#endif
        LoadScene(int sceneIndex)
        {
            var handler = SceneManager.LoadSceneAsync(sceneIndex);
            while (handler.isDone == false) { await AsyncHelper.Skip(); }
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}