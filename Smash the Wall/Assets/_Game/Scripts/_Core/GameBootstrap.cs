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
        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private EventsLogManager _eventsLogManager;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private SceneLoader _sceneLoader;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private ListOfAllScenes _listOfAllScenes;

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

            EnterMainMenu();
        }

        private void EnterMainMenu()
        {
            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }
    }
}