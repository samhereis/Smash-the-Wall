using Configs;
using DataClasses;
using DI;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Sound;
using Tools;
using UI.Canvases;
using UnityEngine;
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
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        [Header("Addressables")]
        [SerializeField] private AssetReferenceAudioClip _loseAudio;

        [Header("Components")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _goToMainMenuButton;

        [Space(10)]
        [SerializeField] private Image _loseInfoBlock;
        [SerializeField] private Image _loseButtonsBlock;

        [Header("Debug")]
        [SerializeField] private SimpleSound _currentLoseAudio;

        public async override void Initialize()
        {
            base.Initialize();

            _currentLoseAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_loseAudio));
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);
            _gameConfigs.isRestart = false;

            SubscribeToEvents();

            SoundPlayer.instance?.TryPlay(_currentLoseAudio);
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