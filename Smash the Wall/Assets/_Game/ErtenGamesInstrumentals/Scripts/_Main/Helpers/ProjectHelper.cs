using DependencyInjection;
using Helpers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Samhereis.Helpers
{

    public sealed class ProjectHelper : MonoBehaviour, INeedDependencyInjection
    {
        [SerializeField] private int _targetFPS = 120;

        private void Awake()
        {
            Application.targetFrameRate = _targetFPS;
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
                await AsyncHelper.Skip();
            }

            string[] folders = Directory.GetDirectories(directory);
            foreach (string folder in folders)
            {
                await DeleteEveryFile(folder);
                await AsyncHelper.Skip();
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