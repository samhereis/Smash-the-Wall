using UnityEngine;

namespace Sound
{
    public class SimpleSoundPlayer : MonoBehaviour
    {
        [SerializeField] private bool _useGlobal = false;
        [SerializeField] private SoundPlayer _soundPlayer;
        [SerializeField] private SimpleSound _soundSettings;

        public void Play()
        {
            if (_useGlobal) SoundPlayer.instance?.TryPlay(_soundSettings); else _soundPlayer?.TryPlay(_soundSettings);
        }
    }
}
