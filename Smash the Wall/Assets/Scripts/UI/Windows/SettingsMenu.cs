using Configs;
using DI;
using InGameStrings;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : CanvasWindowBase
    {
        [Header("DI")]
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;

        [Header("Components")]
        [SerializeField] private Button _closeButton;

        [Space(10)]
        [SerializeField] private Toggle _soundsToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _randomEnviromentToggle;
        [SerializeField] private Toggle _randomPictureToggle;
        [SerializeField] private Slider _gunRotationSpeed;

        [Space(10)]
        [SerializeField] private Image _buttonsInfoBlock;

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _buttonsInfoBlock.color = _uIConfigs.uiBackgroundColor_Standart;

            _randomPictureToggle.isOn = _gameConfigs.gameSettings.randomPictureSettings.currentValue;
            _gunRotationSpeed.value = _gameConfigs.gameplaySettings.gunRotationSpeed.currentValue;
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _closeButton.onClick.AddListener(Close);

            _gunRotationSpeed.onValueChanged.AddListener(SetGunRotationSpeed);
            _randomPictureToggle.onValueChanged.AddListener(SetRandomPicturesEnabled);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _closeButton.onClick.RemoveListener(Close);

            _gunRotationSpeed.onValueChanged.RemoveListener(SetGunRotationSpeed);
            _randomPictureToggle.onValueChanged.RemoveListener(SetRandomPicturesEnabled);
        }

        private void SetGunRotationSpeed(float newGunRotationSpeed)
        {
            _gameConfigs.gameplaySettings.SetGunRotationSpeed(newGunRotationSpeed);
        }

        private void SetRandomPicturesEnabled(bool randomPictureEnabled)
        {
            _gameConfigs.gameSettings.SetRamdonPicturesEnabled(randomPictureEnabled);
        }

        private void Close()
        {
            Disable();
        }
    }
}