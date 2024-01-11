using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    [Serializable]
    public class Sound
    {
        public AudioClip audioClip => GetAudioClip();

        [SerializeField] private List<AudioClip> _audioClips = new();

        [FoldoutGroup("Settings"), SerializeField] public bool isMain { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public bool loop { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public bool disableOthers { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public bool alwaysRandonSound { get; private set; } = true;
        [FoldoutGroup("Settings"), SerializeField] public float volume { get; private set; } = 1;
        [FoldoutGroup("Settings"), SerializeField] public float distance { get; private set; } = 50;

        private AudioClip _currentAudioClip;

        private AudioClip GetAudioClip()
        {
            if (alwaysRandonSound == true) { _currentAudioClip = _audioClips.GetRandom(); }
            if (_currentAudioClip == null) { _currentAudioClip = _audioClips.GetRandom(); }

            return _currentAudioClip;
        }
    }
}