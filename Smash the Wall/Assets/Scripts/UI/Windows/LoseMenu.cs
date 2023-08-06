using Configs;
using DI;
using Events;
using InGameStrings;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoseMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.onLoseEvent)][SerializeField] private EventWithNoParameters _onLose;
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;

        [Header("Components")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _goToMainMenuButton;

        protected void Start()
        {
            (this as IDIDependent).LoadDependencies();

            _onLose.AddListener(OnLose);

            Disable(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _onLose.RemoveListener(OnLose);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);
            _gameConfigs.isRestart = false;

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _restartButton.onClick.AddListener(RestartGame);
            _goToMainMenuButton.onClick.AddListener(GotoMainMenu);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _restartButton.onClick.RemoveListener(RestartGame);
            _goToMainMenuButton.onClick.RemoveListener(GotoMainMenu);
        }

        private void RestartGame()
        {
            _gameConfigs.isRestart = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnLose()
        {
            Enable();
        }

        private void GotoMainMenu()
        {
            SceneManager.LoadSceneAsync(_mainMenuSceneIndex);
        }
    }
}