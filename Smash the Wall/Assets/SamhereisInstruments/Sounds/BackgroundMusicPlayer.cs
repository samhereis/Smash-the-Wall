using DG.Tweening;
using Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public sealed class BackgroundMusicPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _transitionDuration = 1;

        [SerializeField] private List<AssetReferenceAudioClip> _audioClips = new List<AssetReferenceAudioClip>();

        private void OnDestroy()
        {
            _audioSource.DOKill();
        }

        [ContextMenu(nameof(PlayMusic))]
        public async void PlayMusic()
        {
            var audioClip = await AddressablesHelper.GetAssetAsync<AudioClip>(_audioClips.GetRandom());

            _audioSource.clip = audioClip;
            _audioSource.loop = true;

            _audioSource.Play();

            ChangeVolume(1);
        }

        [ContextMenu(nameof(StopMusic))]
        public void StopMusic()
        {
            ChangeVolume(0, () =>
            {
                _audioSource.Stop();

                if (_audioSource.clip != null)
                {
                    AddressablesHelper.Release<AudioClip>(_audioSource.clip);
                }

                _audioSource.clip = null;
            });
        }

        public void ChangeVolume(float volume, Action onComplete = null)
        {
            _audioSource.DOFade(volume, _transitionDuration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}