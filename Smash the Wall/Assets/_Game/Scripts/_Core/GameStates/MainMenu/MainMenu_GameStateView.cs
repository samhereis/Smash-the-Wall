using DependencyInjection;
using InGameStrings;
using Interfaces;
using Managers;
using Servies;
using SO.DataHolders;
using SO.Lists;
using Sound;
using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class MainMenu_GameStateView : GameStateViewBase, IDIDependent, ISubscribesToEvents
    {
        public Action onPlayClicked;

        private MainMenu _mainMenu;
        private SettingsMenu _settingsMenu;

        [Inject] private ListOfAllMenus _listOfAllMenus;
        [Inject] private ListOfAllScenes _listOfAllScenes;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private EventsLogManager _eventsLogManager;
        [Inject] private BackgroundMusicPlayer _backgroundMusicPlayer;
        [Inject(DIStrings.MainMenuSoundPack)] private SoundsPack_DataHolder _mainMenuSoundsPack;

        public override void Initialize()
        {
            base.Initialize();

            DependencyInjector.InjectDependencies(this);

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
        }

        private async void StartGame()
        {
            if (_listOfAllScenes != null)
            {
                await _sceneLoader.LoadSceneAsync(_listOfAllScenes.gameScene);
            }
            else
            {
                int sceneIndex = 2;
                SceneManager.LoadScene(sceneIndex);
            }

            EventsLogManager.LogEvent("PlayButtonClicked");
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}