using DependencyInjection;
using InGameStrings;
using Interfaces;
using SO.DataHolders;
using SO.Lists;
using Sound;
using System;
using UI;

namespace GameState
{
    public class Gameplay_GameStateView : GameStateViewBase, INeedDependencyInjection, ISubscribesToEvents
    {
        public Action onMainMenuClicked;

        private GameplayMenu _gameplayMenu;
        private ShopMenu _shopMenu;
        private PauseMenu _pauseMenu;
        private SettingsMenu _settingsMenu;
        private WinMenu _winMenu;
        private LoseMenu _loseMenu;

        [Inject] private ListOfAllMenus _listOfAllMenus;
        [Inject] private BackgroundMusicPlayer _backgroundMusicPlayer;
        [Inject(DIStrings.MainMenuSoundPack)] private SoundsPack_DataHolder _mainMenuSoundsPack;

        public override void Initialize()
        {
            base.Initialize();

            DependencyContext.InjectDependencies(this);

            SetupUI();

            SubscribeToEvents();

            _backgroundMusicPlayer?.PlayMusic(_mainMenuSoundsPack);
        }

        public override void Dispose()
        {
            base.Dispose();

            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {

        }

        public void UnsubscribeFromEvents()
        {

        }

        private void SetupUI()
        {
            _gameplayMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<GameplayMenu>());
            _shopMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<ShopMenu>());
            _pauseMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<PauseMenu>());
            _settingsMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<SettingsMenu>());
            _winMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<WinMenu>());
            _loseMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<LoseMenu>());

            _gameplayMenu.Initialize(_pauseMenu, _shopMenu);
            _shopMenu.Initialize(_gameplayMenu);
            _pauseMenu.Initialize(_settingsMenu, _gameplayMenu);
            _settingsMenu.Initialize(_pauseMenu);
            _winMenu.Initialize();
            _loseMenu.Initialize();

            _gameplayMenu.Enable();
        }
    }
}