using Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class SoundBase
    {
        public bool hasAudio => _audioClips.Count > 0;
        internal AudioClip audioClip { get { if (_audioClips.Count > 0) return _audioClips.GetRandom(); else return null; } }

        [Required]
        [SerializeField] private List<AudioClip> _audioClips = new();

        [FoldoutGroup("Settings"), SerializeField] public bool isMain { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public bool loop { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public bool disableOthers { get; private set; } = false;
        [FoldoutGroup("Settings"), SerializeField] public float volume { get; private set; } = 1;
        [FoldoutGroup("Settings"), SerializeField] public float distance { get; private set; } = 50;
    }
}