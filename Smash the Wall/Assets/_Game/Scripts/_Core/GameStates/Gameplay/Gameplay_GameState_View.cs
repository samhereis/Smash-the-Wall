using DependencyInjection;
using Interfaces;
using SO.Lists;
using Sounds;
using System;
using UI;

namespace GameState
{
    public class Gameplay_GameState_View : GameState_ViewBase, INeedDependencyInjection, ISubscribesToEvents
    {
        public Action onMainMenuRequested;
        public Action onNextRequested;
        public Action onReplayRequested;

        private GameplayMenu _gameplayMenu;
        private ShopMenu _shopMenu;
        private PauseMenu _pauseMenu;
        private SettingsMenu _settingsMenu;
        private WinMenu _winMenu;
        private LoseMenu _loseMenu;

        [Inject] private ListOfAllMenus _listOfAllMenus;
        [Inject] private BackgroundMusicPlayer _backgroundMusicPlayer;

        private Gameplay_GameState_Model _model;

        public Gameplay_GameState_View(Gameplay_GameState_Model model)
        {
            _model = model;
        }

        public override void Initialize()
        {
            base.Initialize();

            DependencyContext.InjectDependencies(this);

            SetupUI();

            SubscribeToEvents();
        }

        public override void Dispose()
        {
            base.Dispose();

            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _pauseMenu.onGoToMainMenuClicked += RequesGoToMainMenu;

            _winMenu.onMainMenuClicked += RequesGoToMainMenu;
            _winMenu.onNextClicked += RequesNext;

            _loseMenu.onMainMenuClicked += RequesGoToMainMenu;
            _loseMenu.onReplayClicked += RequesReplay;

            _model.onGameplayStatusChanged.AddListener(OnGameplayStatisChanged);
        }

        public void UnsubscribeFromEvents()
        {
            _pauseMenu.onGoToMainMenuClicked -= RequesGoToMainMenu;

            _winMenu.onMainMenuClicked -= RequesGoToMainMenu;
            _winMenu.onNextClicked -= RequesNext;

            _loseMenu.onMainMenuClicked -= RequesGoToMainMenu;
            _loseMenu.onReplayClicked -= RequesReplay;

            _model.onGameplayStatusChanged.RemoveListener(OnGameplayStatisChanged);
        }

        private void SetupUI()
        {
            _gameplayMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<GameplayMenu>());
            _shopMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<ShopMenu>());
            _pauseMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<PauseMenu>());
            _settingsMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<SettingsMenu>());
            _winMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<WinMenu>());
            _loseMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<LoseMenu>());

            _gameplayMenu.Initialize(_shopMenu, _model);
            _shopMenu.Initialize(_gameplayMenu);
            _pauseMenu.Initialize(_settingsMenu, _model);
            _settingsMenu.Initialize(_pauseMenu);
            _winMenu.Initialize(_model);
            _loseMenu.Initialize();
        }

        private void RequesGoToMainMenu()
        {
            onMainMenuRequested?.Invoke();
        }

        private void RequesNext()
        {
            onNextRequested?.Invoke();
        }

        private void RequesReplay()
        {
            onReplayRequested?.Invoke();
        }

        private void OnGameplayStatisChanged(Gameplay_GameState_Model.GameplayState gameplayState)
        {
            switch (gameplayState)
            {
                case Gameplay_GameState_Model.GameplayState.Gameplay:
                    {
                        _gameplayMenu?.Enable();
                        break;
                    }
                case Gameplay_GameState_Model.GameplayState.Pause:
                    {
                        _pauseMenu?.Enable();
                        break;
                    }
                case Gameplay_GameState_Model.GameplayState.Win:
                    {
                        _winMenu?.Enable();
                        break;
                    }
                case Gameplay_GameState_Model.GameplayState.Lose:
                    {
                        _loseMenu?.Enable();
                        break;
                    }
            }
        }
    }
}