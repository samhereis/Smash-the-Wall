using Configs;
using DataClasses;
using DI;
using ECS.Systems.GameState;
using Events;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class WinMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.onWinEvent)][SerializeField] private EventWithNoParameters _onWin;
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.listOfAllPictures)][SerializeField] private ListOfAllPictures _listOfAllPictures;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.onGameSceneLoad)][SerializeField] private EventWithNoParameters _onGameSceneLoad;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        private Button _restartButton;
        private Button _nextLevelButton;
        private Button _goToMainMenuButton;

        private Star_CustomControl _starControl;

        protected override void Start()
        {
            (this as IDIDependent).LoadDependencies();

            base.Start();

            _onWin.AddListener(Open);

            Disable(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _onWin.RemoveListener(Open);
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);

            FindAllUIElements();

            _starControl.starsCount = _gameConfigs.gameplaySettings.winLoseStarSettings.Count;
            _starControl.activeStarsCount = 0;

            float currentPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

            if (currentPercentage <= 0) return;

            for (int i = 0; i < _gameConfigs.gameplaySettings.winLoseStarSettings.Count; i++)
            {
                float percentage = _gameConfigs.gameplaySettings.winLoseStarSettings[i].percentage;

                if (currentPercentage >= percentage)
                {
                    _starControl.activeStarsCount = i + 1;
                }
            }

            _gameConfigs.isRestart = false;

            await AdsManager.instance.Request(AdsStrings.interstitialAd, 5f);
        }

        protected override void FindAllUIElements()
        {
            base.FindAllUIElements();

            _restartButton = baseSettings.root.Q<Button>("RestartButton");
            baseSettings.animatedVisualElements.SafeAdd(_restartButton);

            _nextLevelButton = baseSettings.root.Q<Button>("NextLevelButton");
            baseSettings.animatedVisualElements.SafeAdd(_nextLevelButton);

            _goToMainMenuButton = baseSettings.root.Q<Button>("GoToMainMenuButton");
            baseSettings.animatedVisualElements.SafeAdd(_goToMainMenuButton);

            _starControl = baseSettings.root.Q<Star_CustomControl>("Star_CustomControl");
            baseSettings.animatedVisualElements.SafeAdd(_starControl);
        }

        protected override void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();

            _restartButton.RegisterCallback<ClickEvent>(RestartGame);
            _nextLevelButton.RegisterCallback<ClickEvent>(NextLevel);
            _goToMainMenuButton.RegisterCallback<ClickEvent>(GotoMainMenu);
        }

        protected override void UnSubscribeFromUIEvents()
        {
            base.UnSubscribeFromUIEvents();

            _restartButton.UnregisterCallback<ClickEvent>(RestartGame);
            _nextLevelButton.UnregisterCallback<ClickEvent>(NextLevel);
            _goToMainMenuButton.UnregisterCallback<ClickEvent>(GotoMainMenu);
        }

        private void RestartGame(ClickEvent ev)
        {
            _gameConfigs.isRestart = true;
            LoadLevel(_listOfAllScenes.GetCurrentScene());
        }

        private void NextLevel(ClickEvent evt)
        {
            _listOfAllPictures.SetNextLevel();

            if (GameConfigs.GameSettings.isRamdonEnviromentEnabled)
            {
                LoadLevel(_listOfAllScenes.GetRandomScene());
            }
            else
            {
                LoadLevel(_listOfAllScenes.GetCurrentScene());
            }
        }

        private async void GotoMainMenu(ClickEvent evt)
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }

        private async void LoadLevel(AScene scene)
        {
            await _sceneLoader.LoadSceneAsync(scene);

            AdsShowManager.instance.TryShowInterstitial();
        }
    }
}