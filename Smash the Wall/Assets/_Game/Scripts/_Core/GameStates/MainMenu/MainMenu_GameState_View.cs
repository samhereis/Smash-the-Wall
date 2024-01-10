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
    public class MainMenu_GameState_View : GameState_ViewBase, INeedDependencyInjection, ISubscribesToEvents
    {
        public Action onPlayClicked;
        public Action onQuitClicked;

        private MainMenu _mainMenu;
        private SettingsMenu _settingsMenu;

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
            _mainMenu.onPlayClicked += StartGame;
            _mainMenu.onQuitClicked += Quit;
        }

        public void UnsubscribeFromEvents()
        {
            _mainMenu.onPlayClicked -= StartGame;
            _mainMenu.onQuitClicked -= Quit;
        }

        private void SetupUI()
        {
            _mainMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<MainMenu>());
            _settingsMenu = UnityEngine.Object.Instantiate(_listOfAllMenus.GetMenu<SettingsMenu>());

            _mainMenu.Initialize(_settingsMenu);
            _settingsMenu.Initialize(_mainMenu);

            _mainMenu.Enable();
        }

        private void StartGame()
        {
            onPlayClicked?.Invoke();
        }

        private void Quit()
        {
            onQuitClicked?.Invoke();
        }
    }
}