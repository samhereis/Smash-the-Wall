using DependencyInjection;
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
        [Header("Dependencies")]
        [SerializeField] private SettingsMenu _settingsWindow;
        [SerializeField] private GameplayMenu _gameplayWindow;

        [Header("DI")]
        [Inject][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [Inject] private SceneLoader _sceneLoader;

        [Header("Components")]

        [Required]
        [SerializeField] private Image _buttonsInfoBlock;

        [Required]
        [SerializeField] private Button _resumeButton;

        [Required]
        [SerializeField] private Button _settingsButton;

        [Required]
        [SerializeField] private Button _mainMenuButton;

        public void Initialize()
        {
            if (_settingsWindow == null) { _settingsWindow = FindFirstObjectByType<SettingsMenu>(FindObjectsInactive.Include); }
            if (_gameplayWindow == null) { _gameplayWindow = FindFirstObjectByType<GameplayMenu>(FindObjectsInactive.Include); }
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