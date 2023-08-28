using Configs;
using DataClasses;
using DI;
using Events;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoseMenu : CanvasWindowBase
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        [Header("Components")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _goToMainMenuButton;

        [Space(10)]
        [SerializeField] private Image _loseInfoBlock;
        [SerializeField] private Image _loseButtonsBlock;

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);
            _gameConfigs.isRestart = false;

            SubscribeToEvents();

            _loseInfoBlock.color = _uIConfigs.uiBackgroundColor_Lose;
            _loseButtonsBlock.color = _uIConfigs.uiBackgroundColor_Standart;
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

            LoadLevel(_listOfAllScenes.gameScene);
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