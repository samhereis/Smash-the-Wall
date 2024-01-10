using GameState;
using Sirenix.OdinInspector;
using System;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MenuBase
    {
        public Action onGoToMainMenuClicked;

        [Header("Components")]

        [Required]
        [SerializeField] private Image _buttonsInfoBlock;

        [Required]
        [SerializeField] private Button _resumeButton;

        [Required]
        [SerializeField] private Button _settingsButton;

        [Required]
        [SerializeField] private Button _mainMenuButton;

        private SettingsMenu _settingsWindow;

        private Gameplay_GameState_Model _gameplay_GameState_Model;

        public void Initialize(SettingsMenu settingsWindow, Gameplay_GameState_Model gameplay_GameState_Model)
        {
            _settingsWindow = settingsWindow;
            _gameplay_GameState_Model = gameplay_GameState_Model;
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        override protected void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _resumeButton.onClick.AddListener(Resume);
            _settingsButton.onClick.AddListener(OpenSettings);
            _mainMenuButton.onClick.AddListener(OpenMainMenu);
        }

        override protected void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _resumeButton.onClick.RemoveListener(Resume);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            _mainMenuButton.onClick.RemoveListener(OpenMainMenu);
        }

        private void Resume()
        {
            _gameplay_GameState_Model?.onGameplayStatusChanged?.Invoke(Gameplay_GameState_Model.GameplayState.Gameplay);
        }

        private void OpenSettings()
        {
            _settingsWindow?.Enable();
        }

        private void OpenMainMenu()
        {
            onGoToMainMenuClicked?.Invoke();
        }
    }
}