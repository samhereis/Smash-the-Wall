using Configs;
using DependencyInjection;
using Sirenix.OdinInspector;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : MenuBase, INeedDependencyInjection
    {
        [Required]
        [FoldoutGroup("Components"), SerializeField] private Button[] _closeButtons;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Slider _gunRotationSpeed;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Slider _musicVolume;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Slider _soundsVolume;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Toggle _vibrationToggle;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Toggle _randomPictureToggle;

        [Required]
        [FoldoutGroup("Components"), SerializeField] private Image _buttonsInfoBlock;

        [Inject] private GameConfigs _gameConfigs;

        private MenuBase _openOnClose;

        public void Initialize(MenuBase openOnClose)
        {
            _openOnClose = openOnClose;

            DependencyContext.InjectDependencies(this);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            _gunRotationSpeed.value = _gameConfigs.gameSettings.gunRotationSpeed.currentValue;
            _musicVolume.value = _gameConfigs.gameSettings.musicValue.currentValue;
            _soundsVolume.value = _gameConfigs.gameSettings.soundsVolume.currentValue;
            _vibrationToggle.isOn = _gameConfigs.gameSettings.vibroSettings.currentValue;
            _randomPictureToggle.isOn = _gameConfigs.gameSettings.randomPictureSettings.currentValue;

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            UnsubscribeFromEvents();

            base.Disable(duration);
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            foreach (var button in _closeButtons)
            {
                button.onClick.AddListener(Close);
            }

            _gunRotationSpeed.onValueChanged.AddListener(SetGunRotationSpeed);
            _musicVolume.onValueChanged.AddListener(SetMusicVolume);
            _soundsVolume.onValueChanged.AddListener(SetSoundsVolume);
            _vibrationToggle.onValueChanged.AddListener(SetVibrationEnabled);
            _randomPictureToggle.onValueChanged.AddListener(SetRandomPicturesEnabled);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            foreach (var button in _closeButtons)
            {
                button.onClick.RemoveListener(Close);
            }

            _gunRotationSpeed.onValueChanged.RemoveListener(SetGunRotationSpeed);
            _musicVolume.onValueChanged.RemoveListener(SetMusicVolume);
            _soundsVolume.onValueChanged.RemoveListener(SetSoundsVolume);
            _vibrationToggle.onValueChanged.RemoveListener(SetVibrationEnabled);
            _randomPictureToggle.onValueChanged.RemoveListener(SetRandomPicturesEnabled);
        }

        private void SetGunRotationSpeed(float newGunRotationSpeed)
        {
            _gameConfigs.gameSettings.SetGunRotationSpeed(newGunRotationSpeed);
        }

        private void SetMusicVolume(float musicVolume)
        {
            _gameConfigs.gameSettings.SetMusicVolume(musicVolume);
        }

        private void SetSoundsVolume(float soundsVolume)
        {
            _gameConfigs.gameSettings.SetSoundsVolume(soundsVolume);
        }

        private void SetRandomPicturesEnabled(bool randomPictureEnabled)
        {
            _gameConfigs.gameSettings.SetRamdonPicturesEnabled(randomPictureEnabled);
        }

        private void SetVibrationEnabled(bool isVibrationEnable)
        {
            _gameConfigs.gameSettings.SetVibroEnabled(isVibrationEnable);
        }

        private void Close()
        {
            Disable();

            _openOnClose?.Enable();
        }
    }
}