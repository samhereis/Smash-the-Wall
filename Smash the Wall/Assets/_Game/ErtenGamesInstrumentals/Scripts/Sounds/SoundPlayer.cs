using DependencyInjection;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundPlayer : MonoBehaviour, INeedDependencyInjection, ISelfValidator
    {
        public static SoundPlayer instance { get; private set; }

        public AudioClip currentMainAudioCLip => _mainAudioSource.clip;

        [Required]
        [FoldoutGroup("Componenets"), SerializeField] private AudioSource _mainAudioSource;

        [Required]
        [FoldoutGroup("Componenets"), SerializeField] private List<AudioSource> _audioSourcePool = new List<AudioSource>();

        [Header("Settings")]
        [FoldoutGroup("Settings"), SerializeField] private int _auioSourcePoolCount = 2;
        [FoldoutGroup("Settings"), SerializeField] private bool _isGlobal;

        private void Awake()
        {
            if (_isGlobal == true)
            {
                if (instance == null)
                {
                    instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (instance == this && _isGlobal == true) instance = null;
        }

        public void TryPlay(SoundBase sound)
        {
            if (sound.audioClip == null) return;

            if (sound.isMain)
            {
                TryPlayMain(sound);
            }
            else
            {
                TryPlayPool(sound);
            }
        }

        public void TryPlayMain(SoundBase sound)
        {
            if (_mainAudioSource.clip == sound.audioClip && _mainAudioSource.isPlaying) return;
            StopMain();

            _mainAudioSource.clip = sound.audioClip;
            _mainAudioSource.volume = sound.volume;

            if (sound.distance > 0)
            {
                _mainAudioSource.maxDistance = sound.distance * 2;
                _mainAudioSource.minDistance = 0;
            }

            _mainAudioSource.loop = sound.loop;
            _mainAudioSource.Play();

            if (sound.disableOthers) StopPool();
        }

        public void TryPlayPool(SoundBase sound)
        {
            var freeAudioSurce = _audioSourcePool.Find(x => x.isPlaying == false);

            if (freeAudioSurce == null) freeAudioSurce = _audioSourcePool[0];

            freeAudioSurce.Stop();
            freeAudioSurce.clip = sound.audioClip;
            freeAudioSurce.volume = sound.volume;

            if (sound.distance > 0)
            {
                freeAudioSurce.maxDistance = sound.distance * 2;
                freeAudioSurce.minDistance = 0;
            }

            freeAudioSurce.loop = sound.loop;
            freeAudioSurce.Play();
        }

        public void SetPauseMain(bool pause)
        {
            if (pause == true)
            {
                _mainAudioSource.Pause();
            }
            else
            {
                _mainAudioSource.UnPause();
            }
        }

        public void SetPausePool(bool pause)
        {
            if (pause == true)
            {
                foreach (AudioSource audioSource in _audioSourcePool) audioSource.Pause();
            }
            else
            {
                foreach (AudioSource audioSource in _audioSourcePool) audioSource.UnPause();
            }
        }

        public void SetPauseAllInstanceOF(SoundBase sound, bool pause)
        {
            if (_mainAudioSource.clip == sound.audioClip) SetPauseMain(pause);

            if (pause == true)
            {
                foreach (AudioSource audioSource in _audioSourcePool)
                {
                    if (audioSource.clip == sound.audioClip)
                    {
                        audioSource.Pause();
                    }
                }
            }
            else
            {
                foreach (AudioSource audioSource in _audioSourcePool)
                {
                    if (audioSource.clip == sound.audioClip)
                    {
                        audioSource.UnPause();
                    }
                }
            }
        }

        public void StopMain()
        {
            _mainAudioSource.Stop();
        }

        public void StopPool()
        {
            foreach (AudioSource audioSource in _audioSourcePool) audioSource.Stop();
        }

        public void StopAllInstanceOF(SoundBase sound)
        {
            if (_mainAudioSource.clip == sound.audioClip) StopMain();

            foreach (AudioSource audioSource in _audioSourcePool)
            {
                if (audioSource.clip == sound.audioClip)
                {
                    audioSource.Stop();
                }
            }
        }

        public void Validate(SelfValidationResult result)
        {
            if (_mainAudioSource == null)
            {
                _mainAudioSource = GetComponent<AudioSource>();
            }

            if (_audioSourcePool.Count == 0)
            {
                FillAudioSoursePool();
            }
        }

        [Button]
        private void FillAudioSoursePool()
        {
            if (_auioSourcePoolCount == 0) { _auioSourcePoolCount = 2; }

            _audioSourcePool = GetComponentsInChildren<AudioSource>().ToList();
            _audioSourcePool.RemoveAll(x => x == null || x.gameObject == gameObject);

            if (_audioSourcePool.Count != _auioSourcePoolCount)
            {
                foreach (AudioSource audioSource in _audioSourcePool)
                {
                    if (Application.isPlaying == false)
                    {
                        DestroyImmediate(audioSource.gameObject);
                    }
                    else
                    {
                        Destroy(audioSource.gameObject);
                    }
                }

                _audioSourcePool.Clear();

                for (int i = 0; i < _auioSourcePoolCount; i++)
                {
                    var obj = new GameObject("A sound");
                    obj.transform.parent = transform;
                    obj.AddComponent<AudioSource>();

                    _audioSourcePool.Add(obj.GetComponent<AudioSource>());
                }
            }
        }
    }
}