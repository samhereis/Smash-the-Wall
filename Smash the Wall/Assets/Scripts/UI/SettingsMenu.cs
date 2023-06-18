using Backend;
using Configs;
using DI;
using Helpers;
using System;
using System.Threading.Tasks;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Components")]
        [SerializeField] private CanvasWindowBase _openOnClose;

        [Header("Settings")]
        [SerializeField] private float _settingsHolderAnimationDUration;

        private GameSettingsManager _gameSettingsManager;

        private Button _resumeButton;

        private VisualElement _settingsHolder;

        protected override void Start()
        {
            base.Start();

            Disable(0);
        }

        override protected void FindAllUIElements()
        {
            base.FindAllUIElements();

            _resumeButton = baseSettings.root.Q<Button>("CloseButton");
            baseSettings.animatedVisualElements.SafeAdd(_resumeButton);

            _settingsHolder = baseSettings.root.Q<VisualElement>("SettingsHolder");
            baseSettings.animatedVisualElements.SafeAdd(_settingsHolder);

            _gameSettingsManager?.Dispose();
            _gameSettingsManager = new GameSettingsManager(_settingsHolder);
        }

        override protected void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();
            _resumeButton.RegisterCallback<ClickEvent>(OpenMainMenu);

            _gameSettingsManager?.SubscribeToUIEvents();
        }

        override protected void UnSubscribeFromUIEvents()
        {
            base.UnSubscribeFromUIEvents();
            _resumeButton.UnregisterCallback<ClickEvent>(OpenMainMenu);

            _gameSettingsManager?.UnSubscribeFromEvents();
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);
            await PlaySettingsHolderUpAnimation();
        }

        public override async void Disable(float? duration = null)
        {
            base.Disable(duration);
            await PlaySettingsHolderDownAnimation();
        }

        private void OpenMainMenu(ClickEvent evt)
        {
            _openOnClose.Open();
        }

        private async Task PlaySettingsHolderDownAnimation()
        {
            if (_settingsHolder == null) return;

            float initialValue = _settingsHolder.style.top.value.value;
            float top = initialValue;

            top.TweenFloat(Screen.height, _settingsHolderAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _settingsHolder.style.top = currentValue;
            });

            await AsyncHelper.Delay(_settingsHolderAnimationDUration);
        }

        private async Task PlaySettingsHolderUpAnimation()
        {
            if (_settingsHolder == null) return;

            float initialValue = _settingsHolder.style.top.value.value;
            float top = initialValue;

            top.TweenFloat(0f, _settingsHolderAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _settingsHolder.style.top = currentValue;
            });

            await AsyncHelper.Delay(_settingsHolderAnimationDUration);
        }

        private class GameSettingsManager : IDisposable, IDIDependent
        {
            [DI(InGameStrings.DIStrings.gameConfigs)] private GameConfigs _gameConfigs;

            private VisualElement _root;

            private Toggle _randomEnviromentToggle;
            private Toggle _randomPicturesToggle;

            private Toggle _vibrationToggle;
            private Toggle _musicToggle;
            private Toggle _enviromentSoundsToggle;

            public GameSettingsManager(VisualElement root)
            {
                (this as IDIDependent).LoadDependencies();

                _root = root;

                _randomEnviromentToggle = _root.Q<Toggle>("RandomEnviromentToggle");
                _randomPicturesToggle = _root.Q<Toggle>("RandomPicturesToggle");

                _vibrationToggle = _root.Q<Toggle>("VibrationToggle");
                _musicToggle = _root.Q<Toggle>("MusicToggle");
                _enviromentSoundsToggle = _root.Q<Toggle>("EnviromentSoundsToggle");

                _randomEnviromentToggle.value = GameConfigs.GameSettings.isRamdonEnviromentEnabled;
                _randomPicturesToggle.value = GameConfigs.GameSettings.isRamdonPicturesEnabled;

                _vibrationToggle.value = GameConfigs.GameSettings.isVibroEnabled;
                _musicToggle.value = GameConfigs.GameSettings.isMusicEnabled;
                _enviromentSoundsToggle.value = GameConfigs.GameSettings.areSoundsEnabled;
            }

            public void SubscribeToUIEvents()
            {
                UnSubscribeFromEvents();

                _randomEnviromentToggle.RegisterCallback<ChangeEvent<bool>>(OnRandomEnviromentToggleChanged);
                _randomPicturesToggle.RegisterCallback<ChangeEvent<bool>>(OnRandomPicturesToggleChanged);

                _vibrationToggle.RegisterCallback<ChangeEvent<bool>>(OnVibrationToggleChanged);
                _musicToggle.RegisterCallback<ChangeEvent<bool>>(OnMusicToggleChanged);
                _enviromentSoundsToggle.RegisterCallback<ChangeEvent<bool>>(OnEnviromentSoundsToggleChanged);
            }

            public void UnSubscribeFromEvents()
            {
                _randomEnviromentToggle.UnregisterCallback<ChangeEvent<bool>>(OnRandomEnviromentToggleChanged);
                _randomPicturesToggle.UnregisterCallback<ChangeEvent<bool>>(OnRandomPicturesToggleChanged);

                _vibrationToggle.UnregisterCallback<ChangeEvent<bool>>(OnVibrationToggleChanged);
                _musicToggle.UnregisterCallback<ChangeEvent<bool>>(OnMusicToggleChanged);
                _enviromentSoundsToggle.UnregisterCallback<ChangeEvent<bool>>(OnEnviromentSoundsToggleChanged);
            }

            public void Dispose()
            {
                UnSubscribeFromEvents();
            }

            private void OnRandomEnviromentToggleChanged(ChangeEvent<bool> evt)
            {
                _gameConfigs.gameSettings.SetRamdonEnviromentEnabled(evt.newValue);
            }

            private void OnRandomPicturesToggleChanged(ChangeEvent<bool> evt)
            {
                _gameConfigs.gameSettings.SetRamdonPicturesEnabled(evt.newValue);
            }

            private void OnVibrationToggleChanged(ChangeEvent<bool> evt)
            {
                _gameConfigs.gameSettings.SetVibroEnabled(evt.newValue);
            }

            private void OnMusicToggleChanged(ChangeEvent<bool> evt)
            {
                _gameConfigs.gameSettings.SetMusicEnabled(evt.newValue);
            }

            private void OnEnviromentSoundsToggleChanged(ChangeEvent<bool> evt)
            {
                _gameConfigs.gameSettings.SetSoundsEnabled(evt.newValue);
            }
        }
    }
}