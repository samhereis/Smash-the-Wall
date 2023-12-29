using DG.Tweening;
using Helpers;
using Sirenix.OdinInspector;
using SO.DataHolders;
using System;
using UnityEngine;

namespace Sound
{
    public sealed class BackgroundMusicPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [Required]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _transitionDuration = 1;

        private void OnDestroy()
        {
            _audioSource.DOKill();
        }

        [ContextMenu(nameof(PlayMusic))]
        public void PlayMusic(SoundsPack_DataHolder soundsPack)
        {
            var sound = soundsPack.data.GetRandom();

            _audioSource.clip = sound.audioClip;
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