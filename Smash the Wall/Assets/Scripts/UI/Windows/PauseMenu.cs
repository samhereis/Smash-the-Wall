using DI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Components")]
        [SerializeField] private CanvasWindowBase _settingsWindow;
        [SerializeField] private CanvasWindowBase _gameplayWindow;

        [Header("Settings")]
        [SerializeField] private int _mainSceneIndex = 0;

        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        protected void Start()
        {
            Disable(0);
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

        private void OpenMainMenu()
        {
            SceneManager.LoadSceneAsync(_mainSceneIndex);
        }
    }
}