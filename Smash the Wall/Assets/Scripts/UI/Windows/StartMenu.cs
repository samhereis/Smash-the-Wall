using DI;
using InGameStrings;
using Managers;
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

        [DI(DIStrings.isGameInitialized)][SerializeField] ValueEvent<bool> _isGameInitialized;

        [Header("Components")]
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private Toggle _adsTrackingConsent;
        [SerializeField] private Toggle _analyticsConsent;
        [SerializeField] private Button _startButton;

        public override void Enable(float? duration = null)
        {
            if (_isGameInitialized.value == true)
            {
                _mainMenu.Enable();
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

        private void StartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            _adsManager.SetConsent(_isAdsConsentOn);
            _eventsLogManager.SetDataCollectionStatus(_isAnalyticsSendingOn);

            _mainMenu.Enable();

            _isGameInitialized.ChangeValue(true);
        }
    }
}