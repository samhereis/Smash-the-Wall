using Sirenix.OdinInspector;
using SO;
using Sounds;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AudioConfigs", menuName = "Scriptables/Config/AudioConfigs")]
    public class AudioConfigs : ConfigBase
    {
        [Required]
        [SerializeField] private List<SoundWithName> _sounds = new List<SoundWithName>();

        public override void Initialize()
        {

        }

        public Sounds.Sound GetSound(String_SO name)
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