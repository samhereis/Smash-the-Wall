using Configs;
using DI;
using Events;
using Helpers;
using InGameStrings;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class LoseMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.onLoseEvent)][SerializeField] private EventWithNoParameters _onLose;
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.onGameSceneLoad)][SerializeField] private EventWithNoParameters _onGameSceneLoad;

        private Button _restartButton;
        private Button _goToMainMenuButton;

        protected override void Start()
        {
            (this as IDIDependent).LoadDependencies();

            base.Start();

            _onLose.AddListener(Open);

            Disable(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _onLose.RemoveListener(Open);
        }

        protected override void FindAllUIElements()
        {
            base.FindAllUIElements();

            _restartButton = baseSettings.root.Q<Button>("RestartButton");
            baseSettings.animatedVisualElements.SafeAdd(_restartButton);

            _goToMainMenuButton = baseSettings.root.Q<Button>("GoToMainMenuButton");
            baseSettings.animatedVisualElements.SafeAdd(_goToMainMenuButton);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);
            _gameConfigs.isRestart = false;
        }

        protected override void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();

            _restartButton.RegisterCallback<ClickEvent>(RestartGame);
            _goToMainMenuButton.RegisterCallback<ClickEvent>(GotoMainMenu);
        }

        protected override void UnSubscribeFromUIEvents()
        {
            base.UnSubscribeFromUIEvents();

            _restartButton.UnregisterCallback<ClickEvent>(RestartGame);
            _goToMainMenuButton.UnregisterCallback<ClickEvent>(GotoMainMenu);
        }

        private void RestartGame(ClickEvent ev)
        {
            _gameConfigs.isRestart = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            _onGameSceneLoad.Invoke();
        }

        private void GotoMainMenu(ClickEvent evt)
        {
            SceneManager.LoadSceneAsync(_mainMenuSceneIndex);
        }
    }
}