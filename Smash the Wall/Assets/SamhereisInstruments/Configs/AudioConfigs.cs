using Configs;
using Sound;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AudioConfigs", menuName = "Scriptables/Config/AudioConfigs")]
    public class AudioConfigs : ConfigBase
    {
        [SerializeField] private List<EventBasedSound> _sounds = new List<EventBasedSound>();

        public override void Initialize()
        {

        }

        public SoundBase GetSound(string name)
        {
            var audio = _sounds.Find(x => x.eventName == name);

            if (audio == null)
            {
                return null;
            }

            return audio.sound;
        }
    }
}