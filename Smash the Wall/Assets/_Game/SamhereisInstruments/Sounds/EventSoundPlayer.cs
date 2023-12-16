using Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public class EventSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundPlayer _soundPlayer;
        [SerializeField] private List<EventBasedSound> _sounds = new List<EventBasedSound>();

        private async void OnValidate()
        {
            if (_soundPlayer == null) _soundPlayer = GetComponentInChildren<SoundPlayer>(true);

            foreach (var sound in _sounds)
            {
                sound.sound.volume = 1;
                if (sound.sound.distance == 0) sound.sound.distance = 10;

                await AsyncHelper.Delay();
            }
        }

        private void Awake()
        {
            if (_soundPlayer == null) _soundPlayer = GetComponentInChildren<SoundPlayer>(true);
        }

        public void Play(string eventName)
        {
            var sound = _sounds.Find(x => x.eventName == eventName);
            _soundPlayer?.TryPlay(sound.sound);
        }
    }
}