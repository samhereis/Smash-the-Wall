using DependencyInjection;
using Helpers;
using Interfaces;
using Servies;
using SO.Lists;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class Gameplay_GameState : GameStateBase, INeedDependencyInjection, ISubscribesToEvents
    {
        private Gameplay_GameStateView _view;
        private Gameplay_GameStateModel _model;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;

        public async override void Enter()
        {
            base.Enter();

            DependencyContext.InjectDependencies(this);

            if (_listOfAllScenes != null)
            {
                await _sceneLoader.LoadSceneAsync(_listOfAllScenes.gameScene);
            }
            else
            {
                int sceneIndex = 2;
                var handler = SceneManager.LoadSceneAsync(sceneIndex);
                while (handler.isDone == false) { await AsyncHelper.Skip(); }
            }

            _model = Object.FindObjectOfType<Gameplay_GameStateModel>();

            SetupView();

            SubscribeToEvents();
        }

        public override void Exit()
        {
            UnsubscribeFromEvents();
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

            _view?.Initialize();
            _model?.Initialize();
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