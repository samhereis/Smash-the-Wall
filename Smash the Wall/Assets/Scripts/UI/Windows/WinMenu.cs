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
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

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
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Components")]
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _goToMainMenuButton;

        [SerializeField] private Star_CustomControl _starControl;

        protected void Start()
        {
            (this as IDIDependent).LoadDependencies();

            _onWin.AddListener(OnWin);

            Disable(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _onWin.RemoveListener(OnWin);
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);

            _starControl.SetStarCount(_gameConfigs.gameplaySettings.winLoseStarSettings.Count);

            float currentPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

            if (currentPercentage <= 0) return;

            for (int i = 0; i < _gameConfigs.gameplaySettings.winLoseStarSettings.Count; i++)
            {
                float percentage = _gameConfigs.gameplaySettings.winLoseStarSettings[i].percentage;

                if (currentPercentage >= percentage)
                {
                    _starControl.SetActiveStars(i + 1);
                }
            }

            _gameConfigs.isRestart = false;

            await AdsManager.instance?.Request(AdsStrings.interstitialAd, 5f);

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

            _nextLevelButton.onClick.AddListener(NextLevel);
            _goToMainMenuButton.onClick.AddListener(GotoMainMenu);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _nextLevelButton.onClick.RemoveListener(NextLevel);
            _goToMainMenuButton.onClick.RemoveListener(GotoMainMenu);
        }

        private void OnWin()
        {
            Enable();
        }

        private void NextLevel()
        {
            _listOfAllPictures.SetNextLevel();

            if (GameConfigs.GameSettings.isRamdonEnviromentEnabled)
            {
                LoadLevel(_listOfAllScenes.GetRandomScene());
            }
            else
            {
                _listOfAllScenes.SetNextScene();
                LoadLevel(_listOfAllScenes.GetCurrentScene());
            }
        }

        private async void GotoMainMenu()
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }

        private async void LoadLevel(AScene scene)
        {
            await _sceneLoader.LoadSceneAsync(scene);

            await AsyncHelper.Delay(1000);

            AdsShowManager.instance?.TryShowInterstitial();
        }
    }
}