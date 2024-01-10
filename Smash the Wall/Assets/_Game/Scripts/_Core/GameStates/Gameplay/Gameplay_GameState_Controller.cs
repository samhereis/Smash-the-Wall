using DependencyInjection;
using ECS.Systems;
using ECS.Systems.CollisionUpdators;
using ECS.Systems.GameState;
using ECS.Systems.Spawners;
using Identifiers;
using Interfaces;
using Managers;
using Servies;
using SO.Lists;
using UnityEngine;

namespace GameState
{
    public class Gameplay_GameState_Controller : GameState_ControllerBase, INeedDependencyInjection, ISubscribesToEvents
    {
        private Gameplay_GameState_View _view;
        private Gameplay_GameState_Model _model;

        private SystemsManager _systemsManager;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerIdentifier _playerIdentifier;

        public async override void Enter()
        {
            base.Enter();

            if (_listOfAllScenes != null) { await _sceneLoader.LoadSceneAsync(_listOfAllScenes.gameScene); }
            else { await LoadScene(2); }

            DependencyContext.InjectDependencies(this);

            _model = new Gameplay_GameState_Model();
            _view = new Gameplay_GameState_View(_model);

            WinLoseChecker_System.Initialize(_model.onGameplayStatusChanged);

            _systemsManager = new SystemsManager(new System.Collections.Generic.List<IEnableableSystem>
            {
                PictureSpawner_System.instance,
                DestroyableCollisionUpdator_System.instance,
                ChangeKinematicOnCollided_Updator.instance,
                CheckPicturePieceKinematic_System.instance,
                DestroyDestroyables_System.instance,
                WinLoseChecker_System.instance
            });

            _model?.Initialize();
            _view?.Initialize();

            Object.Instantiate(_playerIdentifier);

            SubscribeToEvents();

            _model.onGameplayStatusChanged.Invoke(Gameplay_GameState_Model.GameplayState.Gameplay);
        }

        public override void Exit()
        {
            UnsubscribeFromEvents();

            _model?.Dispose();
            _view?.Dispose();
            _systemsManager?.Dispose();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _view.onMainMenuRequested += GoToMainMenu;
            _view.onNextRequested += Next;
            _view.onReplayRequested += Replay;
        }

        public void UnsubscribeFromEvents()
        {
            _view.onMainMenuRequested -= GoToMainMenu;
            _view.onNextRequested -= Next;
            _view.onReplayRequested -= Replay;
        }

        private void GoToMainMenu()
        {
            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }

        private void Next()
        {
            _gameStateChanger.ChangeState(new Gameplay_GameState_Controller());
        }

        private void Replay()
        {
            _gameStateChanger.ChangeState(new Gameplay_GameState_Controller());
        }
    }
}