using DependencyInjection;
using Interfaces;
using Managers;
using Servies;
using SO.Lists;

namespace GameState
{
    public class MainMenu_GameState_Controller : GameState_ControllerBase, INeedDependencyInjection, ISubscribesToEvents
    {
        private MainMenu_GameState_View _view;
        private MainMenu_GameState_Model _model;

        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private IGameStateChanger _gameStateChanger;
        [Inject] private SceneLoader _sceneLoader;

        public async override void Enter()
        {
            base.Enter();

            DependencyContext.InjectDependencies(this);

            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);

            _view = new MainMenu_GameState_View();
            _model = new MainMenu_GameState_Model();

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
            EventsLogManager.LogEvent("PlayButtonClicked");

            _gameStateChanger.ChangeState(new Gameplay_GameState_Controller());
        }
    }
}