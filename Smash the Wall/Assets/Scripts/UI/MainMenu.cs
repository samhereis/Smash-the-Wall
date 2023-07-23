using InGameStrings;
using DI;
using Helpers;
using Managers;
using SO.Lists;
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

        private void StartGame(ClickEvent evt)
        {
            int sceneIndex = 1;

            if (_listOfAllScenes != null) { sceneIndex = _listOfAllScenes.GetNext().target; }

            SceneManager.LoadSceneAsync(sceneIndex);
        }

        private void OpenSettings(ClickEvent evt)
        {
            _settingsMenu.Open();
        }
    }
}