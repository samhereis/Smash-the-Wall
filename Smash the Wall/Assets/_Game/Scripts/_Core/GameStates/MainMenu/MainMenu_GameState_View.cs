using DependencyInjection;
using Interfaces;
using SO.Lists;
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