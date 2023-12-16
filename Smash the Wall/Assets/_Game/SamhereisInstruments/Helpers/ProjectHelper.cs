using DI;
using Helpers;
using InGameStrings;
using Managers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Samhereis.Helpers
{
    [RequireComponent(typeof(RemoteConfigManager), typeof(EventsLogManager))]
    public sealed class ProjectHelper : MonoBehaviour, IDIDependent
    {
        private static ProjectHelper _instance;

        [SerializeField] private int _targetFPS = 120;

        [SerializeField] private RemoteConfigManager _remoteConfigManager;

        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;

        private async void Awake()
        {
            Application.targetFrameRate = _targetFPS;

            if (_instance == null)
            {
                while (BindDIScene.isGLoballyInjected == false)
                {
                    await AsyncHelper.Delay();
                }

                (this as IDIDependent).LoadDependencies();

                _remoteConfigManager = GetComponent<RemoteConfigManager>();

                _remoteConfigManager.Initialize();

                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void OnApplicationQuit()
        {
            _gameSaveManager.SaveAll();
        }

#if UNITY_EDITOR

        [MenuItem("Samhereis/Delete All Data")]
        public static async void DeleAllData()
        {
            PlayerPrefs.DeleteAll();
            await TryDeleteAllPersistentDataPath();
        }

#endif

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