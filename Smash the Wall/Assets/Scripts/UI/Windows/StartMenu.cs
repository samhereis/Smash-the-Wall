using DI;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;
using Values;

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

        [DI(DIStrings.isGameInitialized)][SerializeField] ValueEvent<bool> _isGameInitialized;

        [Header("Components")]
        [SerializeField] private Toggle _adsTrackingConsent;
        [SerializeField] private Toggle _analyticsConsent;
        [SerializeField] private Button _startButton;

        protected override async void Awake()
        {
            base.Awake();

            while (BindDIScene.isGLoballyInhected == false)
            {
                await AsyncHelper.Delay(1f);
            }

            Initialize(); 
            
            await AsyncHelper.Delay(1f);

            Time.timeScale = 1f;

            Enable();
        }

        public override async void Enable(float? duration = null)
        {
            if (_isGameInitialized.value == true)
            {
                await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
            }
            else
            {
                if (_isFirstTimeInGame)
                {
                    base.Enable(duration);

                    _startButton.onClick.AddListener(InitializeAndStartGame);
                }
                else
                {
                    StartGame();
                }
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

            _isGameInitialized.ChangeValue(true);
        }
    }
}