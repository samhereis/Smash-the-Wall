using Sirenix.OdinInspector;
using SO;
using Sound;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AudioConfigs", menuName = "Scriptables/Config/AudioConfigs")]
    public class AudioConfigs : ConfigBase
    {
        [Required]
        [SerializeField] private List<SimpleSound> _sounds = new List<SimpleSound>();

        public override void Initialize()
        {

        }

        public SoundBase GetSound(AString_SO name)
        {
            var audio = _sounds.Find(x => x.soundName == name);

            if (audio == null)
            {
                return null;
            }

            return audio;
        }
    }
}