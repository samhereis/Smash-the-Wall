using Configs;
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
using System.Collections.Generic;
using UnityEngine;
using static GameState.Gameplay_GameState_Model;

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
        [Inject] private GameConfigs _gameConfigs;
        [Inject] private ListOfAllPictures _listOfAllPictures;
        [Inject] private GameSaveManager _gameSaveManager;

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

            _model.onGameplayStatusChanged.RemoveListener(OnGameplayStatusChanged);
        }

        public void UnsubscribeFromEvents()
        {
            _view.onMainMenuRequested -= GoToMainMenu;
            _view.onNextRequested -= Next;
            _view.onReplayRequested -= Replay;

            _model.onGameplayStatusChanged.AddListener(OnGameplayStatusChanged);
        }

        private void OnGameplayStatusChanged(GameplayState gameplayState)
        {
            switch (gameplayState)
            {
                case GameplayState.Lose:
                    {
                        OnLose();
                        break;
                    }
            }
        }

        private void OnWin()
        {
            EventsLogManager.LogEvent("LevelCompleted", new Dictionary<string, object>()
            {
               { "LevelMode", _listOfAllPictures.GetCurrent().targetName},
               { "LevelName", _listOfAllPictures.GetCurrent().pictureMode.ToString()},
               { "Stars", _model.CalculateWinStars()},
            });

            _listOfAllPictures.SetNextPicture();
            _gameSaveManager.IncreaseLevelIndex();
        }

        private void OnLose()
        {
            EventsLogManager.LogEvent("LevelFailed", new Dictionary<string, object>()
            {
               { "LevelMode", _listOfAllPictures.GetCurrent().pictureMode.ToString() },
               { "LevelName", _listOfAllPictures.GetCurrent().targetName},
            });
        }

        private void GoToMainMenu()
        {
            _gameConfigs.isRestart = false;
            _gameStateChanger.ChangeState(new MainMenu_GameState_Controller());
        }

        private void Next()
        {
            OnWin();

            _gameConfigs.isRestart = false;
            _gameStateChanger.ChangeState(new Gameplay_GameState_Controller());
        }

        private void Replay()
        {
            _gameConfigs.isRestart = true;
            _gameStateChanger.ChangeState(new Gameplay_GameState_Controller());
        }
    }
}