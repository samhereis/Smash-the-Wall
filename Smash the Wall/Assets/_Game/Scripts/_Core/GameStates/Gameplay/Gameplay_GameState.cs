using DependencyInjection;
using ECS.Systems;
using ECS.Systems.CollisionUpdators;
using ECS.Systems.GameState;
using ECS.Systems.Spawners;
using Interfaces;
using Managers;
using Servies;
using SO.Lists;
using UnityEngine;

namespace GameState
{
    public class Gameplay_GameState : GameStateBase, INeedDependencyInjection, ISubscribesToEvents
    {
        private Gameplay_GameStateView _view;
        private Gameplay_GameStateModel _model;

        private SystemsManager _systemsManager;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;

        public async override void Enter()
        {
            base.Enter();

            if (_listOfAllScenes != null) { await _sceneLoader.LoadSceneAsync(_listOfAllScenes.gameScene); }
            else { await LoadScene(2); }

            DependencyContext.InjectDependencies(this);

            _systemsManager = new SystemsManager(new System.Collections.Generic.List<IEnableableSystem>
            {
                PictureSpawner_System.instance,
                DestroyableCollisionUpdator_System.instance,
                ChangeKinematicOnCollided_Updator.instance,
                CheckPicturePieceKinematic_System.instance,
                DestroyDestroyables_System.instance,
                WinLoseChecker_System.instance
            });

            _model = Object.FindObjectOfType<Gameplay_GameStateModel>();

            SetupView();
            SubscribeToEvents();
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
        }

        public void UnsubscribeFromEvents()
        {

        }

        private void SetupView()
        {
            _view = new Gameplay_GameStateView();

            _model?.Initialize();
            _view?.Initialize();
        }

        private void GoToMainMenu()
        {
            _gameStateChanger.ChangeState(new MainMenu_GameState());
        }

        private void Replay()
        {
            _gameStateChanger.ChangeState(new Gameplay_GameState());
        }
    }
}