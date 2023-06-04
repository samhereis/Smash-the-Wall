using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class LightingSpawner : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private void Awake()
        {
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

            foreach (var subScene in FindObjectsOfType<SubScene>())
            {
                subScene.AutoLoadScene = true;
            }

        }
    }
}