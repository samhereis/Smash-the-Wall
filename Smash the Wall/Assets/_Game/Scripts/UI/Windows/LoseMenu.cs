using Configs;
using DataClasses;
using Helpers;
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
        [Header("Effects")]

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

        private GameConfigs _gameConfigs;
        private SceneLoader _sceneLoader;
        private ListOfAllScenes _listOfAllScenes;
        private AdsShowManager _adsShowManager;

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