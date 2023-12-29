using Sirenix.OdinInspector;
using Helpers;
using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Music
{
    [CreateAssetMenu(fileName = "MusicList", menuName = "Scriptables/MusicList")]
    public class MusicList_SO : ScriptableObject, IInitializable, IDisposable
    {
        public static string MusicFolderPath => $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic)}/DHMMT";

        [Required]
        [ShowInInspector] private List<AudioClip> _musicList = new List<AudioClip>();

        [Required]
        [ShowInInspector] private List<AudioClip> _defaultMusicList;

        public bool hasMusic => count > 0;
        public int count => _musicList.Count;

        public AudioClip GetRandomClip()
        {
            if (_musicList.Count == 0)
            {
                return null;
            }

            return _musicList.GetRandom();
        }

        public async void Initialize()
        {
            Dispose();

            if (Directory.Exists(MusicFolderPath) == false) Directory.CreateDirectory(MusicFolderPath);

            var files = Directory.GetFiles(MusicFolderPath);

            if (files.Length == 0)
            {
                foreach (var item in _defaultMusicList)
                {
                    _musicList.SafeAdd(item);
                }
            }
            else
            {
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + file, AudioType.MPEG))
                        {
                            ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

                            var wait = uwr.SendWebRequest();

                            while (wait.isDone == false) { await AsyncHelper.Skip(); }

                            DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

                            if (dlHandler.isDone) if (dlHandler.audioClip != null) _musicList.SafeAdd(dlHandler.audioClip);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _musicList.Clear();
        }
    }
}