using Configs;
using DI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundPlayer : MonoBehaviour, IDIDependent
    {
        public static SoundPlayer instance { get; private set; }

        public AudioClip currentMainAudioCLip => _mainAudioSource.clip;

        [Header("Componenets")]
        [SerializeField] private AudioSource _mainAudioSource;
        [SerializeField] private List<AudioSource> _audioSourcePool;

        [Header("Settings")]
        [SerializeField] private int _auioSourcePoolCount = 0;
        [SerializeField] private bool _isGlobal;

        [Header("DI")]
        [DI(InGameStrings.DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;

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

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private void OnDestroy()
        {
            if (instance == this && _isGlobal == true) instance = null;
        }

        public void TryPlay(SoundBase sound)
        {
            try
            {
                if (sound.audioClip == null) return;

                if (sound.isMain)
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

                    if (sound.disableOthers) DisableAll();
                }
                else
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
            }
            finally
            {

            }
        }

        public void DisableAll(SoundBase sound)
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

        public void StopMain()
        {
            _mainAudioSource.Stop();
        }

        public void DisableAll()
        {
            foreach (AudioSource audioSource in _audioSourcePool) audioSource.Stop();
        }

        [ContextMenu("Setup")]
        public void Setup()
        {
            if (_mainAudioSource == null) _mainAudioSource = GetComponent<AudioSource>();

            _audioSourcePool = GetComponentsInChildren<AudioSource>().ToList();

            _audioSourcePool.RemoveAll(x => x == null || x.gameObject == gameObject);

            if (_audioSourcePool.Count != _auioSourcePoolCount)
            {
                foreach (AudioSource audioSource in _audioSourcePool) DestroyImmediate(audioSource.gameObject);
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