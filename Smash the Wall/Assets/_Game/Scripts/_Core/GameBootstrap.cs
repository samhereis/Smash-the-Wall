using DependencyInjection;
using Helpers;
using Managers;
using Servies;
using Sirenix.OdinInspector;
using SO.Lists;
using UI;
using UnityEngine;

namespace GameState
{
    public class GameBootstrap : GameBootstrapBase, INeedDependencyInjection
    {
        private string falseString = false.ToString();
        private string trueString = true.ToString();

        public bool hasEverPlayed
        {
            get => PlayerPrefs.GetString(nameof(hasEverPlayed), falseString) == trueString;
            set => PlayerPrefs.SetString(nameof(hasEverPlayed), value.ToString());
        }

        public bool adsTrackingContentEnabled
        {
            get => PlayerPrefs.GetString(nameof(adsTrackingContentEnabled), falseString) == trueString;
            set => PlayerPrefs.SetString(nameof(adsTrackingContentEnabled), value.ToString());
        }

        public bool isSendDataEnabled
        {
            get => PlayerPrefs.GetString(nameof(isSendDataEnabled), falseString) == trueString;
            set => PlayerPrefs.SetString(nameof(isSendDataEnabled), value.ToString());
        }

        [SerializeField] private DependencyContext _dependencyContextPrefab;

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

        private void Start()
        {
            Initialize();
        }

        public async override void Initialize()
        {
            base.Initialize();

            DependencyContext dependencyContext = Instantiate(_dependencyContextPrefab);
            dependencyContext.Initialize();

            while (DependencyContext.isGloballyInjected == false) { await AsyncHelper.DelayFloat(1); }

            DependencyContext.InjectDependencies(this);

            StartMenu startMenu = null;

            if (hasEverPlayed == false)
            {
                startMenu = Instantiate(await _listOfAllMenus.GetMenuAsync<StartMenu>());
                startMenu.onStartClicked += OnStartMenu_StartClicked;
            }
            else
            {
                EnterMainMenu();
            }


            void OnStartMenu_StartClicked()
            {
                startMenu.onStartClicked -= OnStartMenu_StartClicked;

                adsTrackingContentEnabled = startMenu.adsTrackingConsent;

                EnterMainMenu();
            }
        }

        private void EnterMainMenu()
        {
            DontDestroyOnLoad(gameObject);

            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }
    }
}