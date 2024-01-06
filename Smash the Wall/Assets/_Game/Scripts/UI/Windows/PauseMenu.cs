using Servies;
using Sirenix.OdinInspector;
using SO.Lists;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MenuBase
    {
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
        private GameplayMenu _gameplayWindow;

        private ListOfAllScenes _listOfAllScenes;
        private SceneLoader _sceneLoader;

        public void Initialize(SettingsMenu settingsWindow, GameplayMenu gameplayWindow)
        {
            _settingsWindow = settingsWindow;
            _gameplayWindow = gameplayWindow;
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
            _gameplayWindow?.Enable();
        }

        private void OpenSettings()
        {
            _settingsWindow?.Enable();
        }

        private async void OpenMainMenu()
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }
    }
}