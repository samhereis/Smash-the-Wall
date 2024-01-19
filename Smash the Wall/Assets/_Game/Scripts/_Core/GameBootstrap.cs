using DependencyInjection;
using Helpers;
using Managers;
using Servies;
using Sirenix.OdinInspector;
using SO.Lists;
using UnityEngine;

namespace GameState
{
    public class GameBootstrap : GameBootstrapBase, INeedDependencyInjection
    {
        [SerializeField] private DependencyContext _dependencyContextPrefab;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private EventsLogManager _eventsLogManager;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private SceneLoader _sceneLoader;

        [Inject]
        [FoldoutGroup("Depencencies"), SerializeField, ReadOnly] private ListOfAllScenes _listOfAllScenes;

        [Inject]
        [FoldoutGroup("Depencencies"), ShowInInspector, ReadOnly] protected IGameStateChanger _gameStateChanger;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Initialize();
            }
        }

        public async override void Initialize()
        {
            base.Initialize();

            DependencyContext dependencyContext = Instantiate(_dependencyContextPrefab);
            dependencyContext.Initialize();

            while (DependencyContext.isGloballyInjected == false) { await AsyncHelper.DelayFloat(1); }

            DependencyContext.InjectDependencies(this);

            EnterMainMenu();
        }

        private void EnterMainMenu()
        {
            DontDestroyOnLoad(gameObject);

            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }
    }
}