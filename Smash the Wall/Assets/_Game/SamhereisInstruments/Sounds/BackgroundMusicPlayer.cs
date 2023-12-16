using DG.Tweening;
using Helpers;
using SO.DataHolders;
using System;
using UnityEngine;

namespace Sound
{
    public sealed class BackgroundMusicPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _transitionDuration = 1;

        private void OnDestroy()
        {
            _audioSource.DOKill();
        }

        [ContextMenu(nameof(PlayMusic))]
        public async void PlayMusic(SoundsPack_DataHolder soundsPack)
        {
            var audioClip = await AddressablesHelper.GetAssetAsync<AudioClip>(soundsPack.data.GetRandom());

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
            }).SetUpdate(true);
        }
    }
}