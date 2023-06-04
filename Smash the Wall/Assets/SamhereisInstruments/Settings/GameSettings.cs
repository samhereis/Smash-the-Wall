using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings instance;

        [Header("Sensitivity")]
        [SerializeField] private FloatSetting_SO _sensitivity;
        [SerializeField] private Slider _sensitivitySlider;

        [Header("Audio")]
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private FloatSetting_SO _volume;
        [SerializeField] private Slider _volumeSlider;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            _volumeSlider.value = _volume.currentValue;
            _sensitivitySlider.value = _sensitivity.currentValue;
        }

        public void ApplyVolume()
        {
            _mixer.SetFloat(_volume.name, _volume.currentValue);
        }

        public void ChangeVolume(Slider slider)
        {
            _volume.SetData(slider.value);
            ApplyVolume();
        }

        public void ChangeSensitivity(Slider slider)
        {
            _sensitivity.SetData(slider.value);
        }

        public void DeleteAllGameData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}