using DI;
using Helpers;
using System.Threading.Tasks;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Components")]
        [SerializeField] private CanvasWindowBase _openOnResume;
        [SerializeField] private CanvasWindowBase _settingsWindow;

        [Header("Settings")]
        [SerializeField] private int _mainSceneIndex = 0;
        [SerializeField] private float _upDownAnimationDUration = 1;

        private Button _resumeButton;
        private Button _settingsButton;
        private Button _mainMenuButton;

        private VisualElement _pauseButtonsHolder;
        private VisualElement _pauseMenuLabelHolder;

        protected override void Start()
        {
            base.Start();

            Disable(0);
        }

        override protected void FindAllUIElements()
        {
            base.FindAllUIElements();

            _resumeButton = baseSettings.root.Q<Button>("ResumeButton");
            baseSettings.animatedVisualElements.SafeAdd(_resumeButton);

            _settingsButton = baseSettings.root.Q<Button>("SettingsButton");
            baseSettings.animatedVisualElements.SafeAdd(_settingsButton);

            _mainMenuButton = baseSettings.root.Q<Button>("MainMenuButton");
            baseSettings.animatedVisualElements.SafeAdd(_mainMenuButton);

            _pauseButtonsHolder = baseSettings.root.Q<VisualElement>("PauseButtonsHolder");
            baseSettings.animatedVisualElements.SafeAdd(_pauseButtonsHolder);

            _pauseMenuLabelHolder = baseSettings.root.Q<VisualElement>("PauseMenuLabelHolder");
            baseSettings.animatedVisualElements.SafeAdd(_pauseButtonsHolder);
        }

        override protected void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();

            _resumeButton.RegisterCallback<ClickEvent>(Resume);
            _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
            _mainMenuButton.RegisterCallback<ClickEvent>(OpenMainMenu);
        }

        override protected void UnSubscribeFromEvents()
        {
            base.UnSubscribeFromEvents();

            _resumeButton.UnregisterCallback<ClickEvent>(Resume);
            _settingsButton.UnregisterCallback<ClickEvent>(OpenSettings);
            _mainMenuButton.UnregisterCallback<ClickEvent>(OpenMainMenu);
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);
            await PlayOnEnableAnimation();
        }

        public override async void Disable(float? duration = null)
        {
            base.Disable(duration);
            await PlayOnDisableAnimation();
        }

        private void Resume(ClickEvent evt)
        {
            _openOnResume.Open();
        }

        private void OpenSettings(ClickEvent evt)
        {
            _settingsWindow.Open();
        }

        private void OpenMainMenu(ClickEvent evt)
        {
            SceneManager.LoadSceneAsync(_mainSceneIndex);
        }

        private async Task PlayOnEnableAnimation()
        {
            if (_pauseButtonsHolder == null) return;

            _pauseMenuLabelHolder.style.top.value.value.TweenFloat(0f, _upDownAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _pauseMenuLabelHolder.style.top = currentValue;
            });

            _pauseButtonsHolder.style.top.value.value.TweenFloat(0f, _upDownAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _pauseButtonsHolder.style.top = currentValue;
            });

            await AsyncHelper.Delay(_upDownAnimationDUration);
        }

        private async Task PlayOnDisableAnimation()
        {
            if (_pauseButtonsHolder == null) return;

            _pauseMenuLabelHolder.style.top.value.value.TweenFloat(-Screen.height, _upDownAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _pauseMenuLabelHolder.style.top = currentValue;
            });

            _pauseButtonsHolder.style.top.value.value.TweenFloat(Screen.height, _upDownAnimationDUration, onUpdateCallback: (currentValue) =>
            {
                _pauseButtonsHolder.style.top = currentValue;
            });

            await AsyncHelper.Delay(_upDownAnimationDUration);
        }
    }
}