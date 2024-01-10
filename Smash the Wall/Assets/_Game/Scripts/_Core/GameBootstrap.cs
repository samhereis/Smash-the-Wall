using DependencyInjection;
using Helpers;
using Managers;
using Services;
using Servies;
using Sirenix.OdinInspector;
using SO.Lists;
using UI;
using UnityEngine;

namespace GameState
{
    public class GameBootstrap : GameBootstrapBase, INeedDependencyInjection
    {
        private const string _isFirstTimeInGameStrings = "IsFirstTimeInGame";
        private const string _adsTrackingOnString = "adsTrackingOnString";
        private const string _analyticsOnString = "analyticsOn";
        private const string _falseString = "false";
        private const string _trueStrings = "true";

        // ---

        public bool isFirstTimeInGame => PlayerPrefs.GetString(_isFirstTimeInGameStrings, _trueStrings) == _trueStrings;
        public bool isAdsConsentOn => PlayerPrefs.GetString(_adsTrackingOnString, _trueStrings) == _trueStrings;
        public bool isAnalyticsSendingOn => PlayerPrefs.GetString(_analyticsOnString, _trueStrings) == _trueStrings;

        // ---

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private AdsManager _adsManager;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private EventsLogManager _eventsLogManager;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private SceneLoader _sceneLoader;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private ListOfAllScenes _listOfAllScenes;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private ListOfAllMenus _listOfAllMenus;

        [Inject]
        [FoldoutGroup("Depencencies"), ShowInInspector, ReadOnly] protected IGameStateChanger _gameStateChanger;

        private StartMenu _startMenu;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        public async override void Initialize()
        {
            base.Initialize();

            while (DependencyContext.isGloballyInjected == false) { await AsyncHelper.Skip(); }

            DependencyContext.InjectDependencies(this);

            if (isFirstTimeInGame == true)
            {
                _startMenu = Instantiate(_listOfAllMenus.GetMenu<StartMenu>());
                _startMenu.onStartClicked += StartGame;
            }
            else
            {
                EnterMainMenu();
            }
        }

        private void StartGame()
        {
            _startMenu.onStartClicked -= StartGame;

            UpdateAnalyticsPreferences();

            EnterMainMenu();
        }

        private void EnterMainMenu()
        {
            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }

        private void UpdateAnalyticsPreferences()
        {
            PlayerPrefs.SetString(_adsTrackingOnString, _startMenu.adsTrackingConsent == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_analyticsOnString, _startMenu.analyticsConsent == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_isFirstTimeInGameStrings, _falseString);

            _adsManager.SetConsent(isAdsConsentOn);
            _eventsLogManager.SetDataCollectionStatus(isAnalyticsSendingOn);
        }
    }
}