using DependencyInjection;
using Servies;
using SO.Lists;
using UnityEngine;

namespace GameState
{
    public class MainMenu_GameState : GameStateBase, IDIDependent
    {
        private MainMenu_GameStateView _view;
        private MainMenu_GameStateModel _model;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;

        public async override void Enter()
        {
            base.Enter();

            DependencyInjector.InjectDependencies(this);

            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);

            _view = new MainMenu_GameStateView();
            _model = Object.FindObjectOfType<MainMenu_GameStateModel>();

            _view?.Initialize();
            _model?.Initialize();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void Play()
        {

        }
    }
}