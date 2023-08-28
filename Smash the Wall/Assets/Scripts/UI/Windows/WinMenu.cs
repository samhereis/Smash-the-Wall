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
    public class WinMenu : CanvasWindowBase
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        [Header("Components")]
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _goToMainMenuButton;

        [Space(10)]
        [SerializeField] private Image _winInfoBlock;
        [SerializeField] private Image _buttonsInfoBlock;

        [Space(10)]
        [SerializeField] private Star_CustomControl _starControl;

        public override void Enable(float? duration = null)
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

            _winInfoBlock.color = _uIConfigs.uiBackgroundColor_Win;
            _buttonsInfoBlock.color = _uIConfigs.uiBackgroundColor_Standart;

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

        private void NextLevel()
        {
            if (GameConfigs.GameSettings.isRamdonEnviromentEnabled)
            {
                LoadLevel(_listOfAllScenes.GetRandomScene());
            }
            else
            {
                LoadLevel(_listOfAllScenes.GetCurrentScene());
            }
        }

        private void GotoMainMenu()
        {
            LoadLevel(_listOfAllScenes.mainMenuScene, false);
        }

        private async void LoadLevel(AScene scene, bool showInterstitial = true)
        {
            await _sceneLoader.LoadSceneAsync(scene);

            if (showInterstitial == true)
            {
                await AsyncHelper.Delay(2000);

                _adsShowManager?.TryShowInterstitial();
            }
        }
    }
}