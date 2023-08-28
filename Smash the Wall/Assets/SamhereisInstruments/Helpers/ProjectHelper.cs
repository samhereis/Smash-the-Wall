using Helpers;
using Managers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Samhereis.Helpers
{
    [RequireComponent(typeof(RemoteConfigManager), typeof(EventsLogManager))]
    public sealed class ProjectHelper : MonoBehaviour
    {
        [SerializeField] private int _targetFPS = 120;

        [SerializeField] private RemoteConfigManager _remoteConfigManager;
        [SerializeField] private EventsLogManager _eventsLogManager;

        private void Awake()
        {
            Application.targetFrameRate = _targetFPS;

            if (FindObjectOfType<ProjectHelper>(true) == null)
            {
                DontDestroyOnLoad(gameObject);

                _remoteConfigManager = GetComponent<RemoteConfigManager>();
                _eventsLogManager = GetComponent<EventsLogManager>();

                _remoteConfigManager.Initialize();
                _eventsLogManager.Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            GameSaveManager.SaveAll();
        }

        [ContextMenu("DeleteAllPersistentDataPath")]
        public async void DeleteAllPersistentDataPath()
        {
            await TryDeleteAllPersistentDataPath();
        }

        public static async Task TryDeleteAllPersistentDataPath()
        {
            await DeleteEveryFile(Application.persistentDataPath);
        }

        private static async Task DeleteEveryFile(string directory)
        {
            string[] filePaths = Directory.GetFiles(directory);
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
                await AsyncHelper.Delay();
            }

            string[] folders = Directory.GetDirectories(directory);
            foreach (string folder in folders)
            {
                await DeleteEveryFile(folder);
                await AsyncHelper.Delay();
                Directory.Delete(folder);
            }
        }

        [ContextMenu("OpenPersistentDataPath")]
        public void OpenPersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }
    }
}