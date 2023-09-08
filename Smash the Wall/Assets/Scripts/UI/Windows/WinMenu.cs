using Configs;
using DataClasses;
using DI;
using ECS.Systems.GameState;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static DataClasses.Enums;

namespace UI
{
    public class WinMenu : CanvasWindowBase
    {
        [Header("Settings")]
        [SerializeField] private int _mainMenuSceneIndex;

        [Header("DI")]
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.listOfAllPictures)][SerializeField] private ListOfAllPictures _listOfAllPictures;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        [Header("Addressables")]
        [SerializeField] AssetReferenceSprite[] _winEmojis;

        [Header("Components")]
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _goToMainMenuButton;

        [Space(10)]
        [SerializeField] private Image _winInfoBlock;
        [SerializeField] private Image _buttonsInfoBlock;

        [Space(10)]
        [SerializeField] private Image _winEmoji;
        [SerializeField] private Star_CustomControl _starControl;

        private PictureMode _currentPictureMode;

        protected override void Awake()
        {
            base.Awake();

            _currentPictureMode = _listOfAllPictures.GetCurrent().pictureMode;
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _winEmoji.gameObject.SetActive(false);
            _starControl.gameObject.SetActive(false);

            _gameConfigs.isRestart = false;

            switch (_currentPictureMode)
            {
                case Enums.PictureMode.DestroyBorder:
                    {
                        _starControl.gameObject.SetActive(true);

                        _starControl.SetStarCount(_gameConfigs.gameSettings.winLoseStarSettings.Count);
                        _starControl.SetActiveStars(CalculateStars());

                        break;
                    }
                case Enums.PictureMode.DestroyWholeObject:
                    {
                        _winEmoji.sprite = await AddressablesHelper.GetAssetAsync<Sprite>(_winEmojis.GetRandom());

                        _winEmoji.gameObject.SetActive(true);

                        break;
                    }
                case Enums.PictureMode.Coloring:
                    {
                        break;
                    }
            }
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

        public int CalculateStars()
        {
            int stars = 0;
            float currentPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

            for (int i = 0; i < _gameConfigs.gameSettings.winLoseStarSettings.Count; i++)
            {
                float percentage = _gameConfigs.gameSettings.winLoseStarSettings[i].percentage;

                if (currentPercentage >= percentage)
                {
                    stars = i + 1;
                }
            }

            return stars;
        }

        private void NextLevel()
        {
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