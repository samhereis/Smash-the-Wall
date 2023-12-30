using Configs;
using DataClasses;
using DependencyInjection;
using Helpers;
using Managers;
using Services;
using Servies;
using Sirenix.OdinInspector;
using SO.Lists;
using Sound;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoseMenu : MenuBase
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [Inject][SerializeField] private GameConfigs _gameConfigs;
        [Inject][SerializeField] private SceneLoader _sceneLoader;
        [Inject][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [Inject][SerializeField] private AdsShowManager _adsShowManager;

        [Header("Addressables")]

        [Required]
        [SerializeField] private SimpleSound _loseAudio;

        [Header("Components")]

        [Required]
        [SerializeField] private Button _restartButton;

        [Required]
        [SerializeField] private Button _goToMainMenuButton;

        [Required]
        [SerializeField] private Image _loseInfoBlock;

        [Required]
        [SerializeField] private Image _loseButtonsBlock;

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);
            _gameConfigs.isRestart = false;

            SubscribeToEvents();

            SoundPlayer.instance?.TryPlay(_loseAudio);
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
                await AsyncHelper.DelayFloat(2000);

                _adsShowManager?.TryShowInterstitial();
            }
        }
    }
}