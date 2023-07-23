using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Helpers
{
    public static class SceneLoader
    {
        public static async Task LoadScene(string sceneName)
        {
            var handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (handle.isDone == false)
            {
                await AsyncHelper.Delay();
            }
        }

        public static async Task LoadScene(int sceneIndex)
        {
            var handle = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

            while (handle.isDone == false)
            {
                await AsyncHelper.Delay();
            }
        }
    }
}