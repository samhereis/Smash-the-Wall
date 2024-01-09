using DependencyInjection;
using Interfaces;
using Managers;
using Servies;
using SO.Lists;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class MainMenu_GameState : GameStateBase, INeedDependencyInjection, ISubscribesToEvents
    {
        private MainMenu_GameStateView _view;
        private MainMenu_GameStateModel _model;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;

        public async override void Enter()
        {
            base.Enter();

            DependencyContext.InjectDependencies(this);

            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);

            _view = new MainMenu_GameStateView();
            _model = Object.FindObjectOfType<MainMenu_GameStateModel>();

            _view?.Initialize();
            _model?.Initialize();

            SubscribeToEvents();
        }

        public override void Exit()
        {
            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _view.onPlayClicked += Play;
        }

        public void UnsubscribeFromEvents()
        {
            _view.onPlayClicked -= Play;
        }

        private void Play()
        {
            //EventsLogManager.LogEvent("PlayButtonClicked");

            _gameStateChanger.ChangeState(new Gameplay_GameState());
        }
    }
}