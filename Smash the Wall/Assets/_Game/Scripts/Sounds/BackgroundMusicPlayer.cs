using Configs;
using DependencyInjection;
using DG.Tweening;
using Sirenix.OdinInspector;
using SO;
using System;
using UnityEngine;

namespace Sounds
{
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [Required]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _transitionDuration = 1;

        [Inject] private AudioConfigs _audioConfigs;

        private Sound _currentMusic;

        private void OnDestroy()
        {
            _audioSource.DOKill();
        }

        [ContextMenu(nameof(PlayMusic))]
        public void PlayMusic(Sound_String_SO soundsPack)
        {
            if (soundsPack == null) return;
            if (_audioConfigs == null) return;

            _currentMusic = _audioConfigs.GetSound(soundsPack);

            _audioSource.clip = _currentMusic.audioClip;
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