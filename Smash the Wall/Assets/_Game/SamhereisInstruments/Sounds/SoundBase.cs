using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class SoundBase
    {
        internal AudioClip audioClip { get { if (_audioClips.Length > 0) return _audioClips[UnityEngine.Random.Range(0, _audioClips.Length)]; else return null; } }

        [SerializeField] internal AudioClip[] _audioClips;
        [SerializeField] internal bool isMain = false;
        [SerializeField] internal bool loop = false;
        [SerializeField] internal bool disableOthers = false;
        [SerializeField] internal float volume = 1;
        [SerializeField] internal float distance = 50;

        public bool hasAudio => _audioClips.Length > 0;

        public void SetAudioClip(AudioClip audioClip)
        {
            _audioClips = new AudioClip[1];
            _audioClips[0] = audioClip;
        }
    }
}