using DependencyInjection;
using Helpers;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace Music
{
    public class PlayingMusicData : MonoBehaviour, IDIDependent
    {
        [Required]
        [Inject]
        [ShowInInspector] private SpectrumData _spectrumData;

        [Required]
        [Inject]
        [ShowInInspector] private MusicList_SO _musicList;

        [Header("Components")]
        [Required]
        [ShowInInspector] private AudioSource _audioSource;

        [Header("Debug")]
        [ShowInInspector] private bool _isCheckingForAudio = false;

        private void Awake()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            DependencyInjector.InjectDependencies(this);

            CheckForAudio(destroyCancellationToken);
        }

        private void Update()
        {
            if (_spectrumData != null)
            {
                _spectrumData.SetSpectrumWidth(_audioSource);
            }
        }

        public void PauseMusic(bool pause)
        {
            if (pause) _audioSource.Pause(); else _audioSource.UnPause();
        }

        private async void CheckForAudio(CancellationToken cancellationTokenSource)
        {
            if (_isCheckingForAudio) return;
            _isCheckingForAudio = true;

            while (cancellationTokenSource.IsCancellationRequested == false)
            {
                if ((_audioSource.isPlaying == false || _audioSource.clip == null) && _musicList.count > 0)
                {
                    _audioSource.clip = null;
                    _audioSource.clip = _musicList.GetRandomClip();
                    _audioSource.Play();
                }

                await AsyncHelper.DelayFloat(1f);
            }

            _isCheckingForAudio = false;
        }
    }
}