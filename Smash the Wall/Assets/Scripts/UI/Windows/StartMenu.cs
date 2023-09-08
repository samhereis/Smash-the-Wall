using DG.Tweening;
using DI;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using System.Collections.Generic;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : CanvasWindowBase
    {
        private const string _isFirstTimeInGameStrings = "IsFirstTimeInGame";
        private const string _adsTrackingOnString = "adsTrackingOnString";
        private const string _analyticsOnString = "analyticsOn";
        private const string _falseString = "false";
        private const string _trueStrings = "true";

        private bool _isFirstTimeInGame => PlayerPrefs.GetString(_isFirstTimeInGameStrings, _trueStrings) == _trueStrings;
        private bool _isAdsConsentOn => PlayerPrefs.GetString(_adsTrackingOnString, _trueStrings) == _trueStrings;
        private bool _isAnalyticsSendingOn => PlayerPrefs.GetString(_analyticsOnString, _trueStrings) == _trueStrings;

        [Header("DI")]
        [DI(DIStrings.adsManager)][SerializeField] private AdsManager _adsManager;
        [DI(DIStrings.eventsLogManager)][SerializeField] private EventsLogManager _eventsLogManager;

        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;

        [Header("Addressables")]
        [SerializeField] private List<AssetReferenceSprite> _backgroundSprites = new List<AssetReferenceSprite>();

        [Header("Components")]
        [SerializeField] private Toggle _adsTrackingConsent;
        [SerializeField] private Toggle _analyticsConsent;
        [SerializeField] private Button _startButton;
        [SerializeField] private Transform _buttonsHolder;

        [Space(10)]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        protected override async void Awake()
        {
            Time.timeScale = 1f;

            base.Awake();

            while (BindDIScene.isGLoballyInhected == false)
            {
                await AsyncHelper.Delay(1f);
            }

            _buttonsHolder.gameObject.SetActive(false);

            Initialize();

            Enable();
        }

        private void Update()
        {
            _gridLayoutGroup.cellSize = new Vector2(Screen.height, Screen.height);
        }

        public override async void Enable(float? duration = null)
        {
            if (_isFirstTimeInGame)
            {
                try
                {
                    _backgroundSprites.RemoveNulls();
                    _backgroundImage.sprite = await AddressablesHelper.GetAssetAsync<Sprite>(_backgroundSprites.GetRandom());
                }
                finally
                {
                    base.Enable(duration);

                    await AsyncHelper.Delay(2f);

                    _buttonsHolder.transform.localScale = Vector3.zero;
                    _buttonsHolder.gameObject.SetActive(true);
                    _buttonsHolder.transform.DOScale(1, 1);

                    _startButton.onClick.AddListener(InitializeAndStartGame);
                }
            }
            else
            {
                StartGame();
            }
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            _startButton.onClick.RemoveListener(InitializeAndStartGame);
        }

        private void InitializeAndStartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            PlayerPrefs.SetString(_adsTrackingOnString, _adsTrackingConsent.isOn == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_analyticsOnString, _analyticsConsent.isOn == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_isFirstTimeInGameStrings, _falseString);

            StartGame();
        }

        private async void StartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            _adsManager.SetConsent(_isAdsConsentOn);
            _eventsLogManager.SetDataCollectionStatus(_isAnalyticsSendingOn);

            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }
    }
}