using DI;
using Helpers;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : CanvasWindowBase, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.noAdsManager)][SerializeField] private NoAdsManager _noAdsManager;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Components")]
        [SerializeField] private SettingsMenu _settingsMenu;

        private Button _playButton;
        private Button _settingsButton;
        private Button _quitButton;
        private Button _noAdsButton;

        protected override async void Start()
        {
            (this as IDIDependent).LoadDependencies();

            base.Start();

            Disable(0);
            await AsyncHelper.Delay(1000);
            Open();
        }

        protected override void FindAllUIElements()
        {
            base.FindAllUIElements();

            _playButton = baseSettings.root.Q<Button>("PlayButton");
            baseSettings.animatedVisualElements.SafeAdd(_playButton);

            _settingsButton = baseSettings.root.Q<Button>("SettingsButton");
            baseSettings.animatedVisualElements.SafeAdd(_settingsButton);

            _quitButton = baseSettings.root.Q<Button>("QuitButton");
            baseSettings.animatedVisualElements.SafeAdd(_quitButton);

            _noAdsButton = baseSettings.root.Q<Button>("NoAdsButton");
            baseSettings.animatedVisualElements.SafeAdd(_noAdsButton);
            _noAdsManager.AddNoAdsButton(_noAdsButton);
        }

        protected override void UnSubscribeFromUIEvents()
        {
            base.UnSubscribeFromUIEvents();

            _playButton.UnregisterCallback<ClickEvent>(StartGame);
            _settingsButton.UnregisterCallback<ClickEvent>(OpenSettings);
        }

        protected override void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();

            _playButton.RegisterCallback<ClickEvent>(StartGame);
            _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
        }

        private async void StartGame(ClickEvent evt)
        {
            if (_listOfAllScenes != null)
            {
                await _sceneLoader.LoadSceneAsync(_listOfAllScenes.GetNextScene());
            }
            else
            {
                int sceneIndex = 1;
                SceneManager.LoadScene(sceneIndex);
            }

        }

        private void OpenSettings(ClickEvent evt)
        {
            _settingsMenu.Open();
        }
    }
}