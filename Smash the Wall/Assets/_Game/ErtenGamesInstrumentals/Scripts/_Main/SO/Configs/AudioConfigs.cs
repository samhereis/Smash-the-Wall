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
        [SerializeField] private List<SoundWithName> _uiSounds = new List<SoundWithName>();

        [Required]
        [SerializeField] private List<SoundWithName> _gunSounds = new List<SoundWithName>();

        [Required]
        [SerializeField] private List<SoundWithName> _enviromentSounds = new List<SoundWithName>();

        private List<SoundWithName> _allSounds = new List<SoundWithName>();

        public override void Initialize()
        {
            _allSounds.Clear();

            _allSounds.AddRange(_uiSounds);
            _allSounds.AddRange(_gunSounds);
            _allSounds.AddRange(_enviromentSounds);
        }

        public Sound GetSound(String_SO name)
        {
            var audio = _allSounds.Find(x => x.soundName == name);

            if (audio == null)
            {
                return null;
            }

            return audio;
        }
    }
}