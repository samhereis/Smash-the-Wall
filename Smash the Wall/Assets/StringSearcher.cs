using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Helpers
{
    public class StringSearcher : MonoBehaviour
    {
        private enum SearchMode { Contains, DoesNotContain }

        [SerializeField, FolderPath] private string _folderPath = "Assets";
        [SerializeField] private string _checkFor = "OnValidate()";
        [SerializeField] private string _extension = "cs";
        [SerializeField] private SearchMode _searchMode = SearchMode.Contains;
        [SerializeField] private bool _asynchronous = false;

        [Space(20)]
        [SerializeField, ReadOnly] private Object _currentCheckedObject;
        [SerializeField, ReadOnly] private List<Object> _files = new List<Object>();

        [Button]
        private async void SearchForScripts()
        {
            _files.Clear();

            if (Directory.Exists(_folderPath))
            {
                var files = await GetAllFiles(_folderPath, _checkFor, "*." + _extension, _searchMode);
                if (files.IsNullOrEmpty()) { return; }

                foreach (string scriptPath in files)
                {
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(scriptPath);
                    _files.Add(obj);

                    if (_asynchronous == true) await AsyncHelper.Skip();
                }
            }
        }

        private async Task<List<string>> GetAllFiles(string folderPath, string checkFor, string extension, SearchMode searchMode)
        {
            string[] scriptFiles = Directory.GetFiles(folderPath, extension, SearchOption.AllDirectories);
            List<string> result = new List<string>();

            foreach (string scriptFile in scriptFiles)
            {
                string scriptContent = File.ReadAllText(scriptFile);

                try
                {
                    _currentCheckedObject = AssetDatabase.LoadAssetAtPath<Object>(scriptFile);
                }
                finally
                {
                    switch (searchMode)
                    {
                        case SearchMode.Contains:
                            {
                                if (scriptContent.Contains(checkFor)) { result.Add(scriptFile); }

                                break;
                            }
                        case SearchMode.DoesNotContain:
                            {
                                if (scriptContent.Contains(checkFor) == false) { result.Add(scriptFile); }

                                break;
                            }
                    }

                    if (_asynchronous == true) await AsyncHelper.Skip();
                }
            }

            return result;
        }
    }
}