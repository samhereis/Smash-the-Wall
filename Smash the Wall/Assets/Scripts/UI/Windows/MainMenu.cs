using Configs;
using DI;
using InGameStrings;
using Managers;
using SO.Lists;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : CanvasWindowBase
    {
        [Header("DI")]
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;

        [Header("Components")]
        [SerializeField] private SettingsMenu _settingsMenu;

        [Space(10)]
        [SerializeField] private Image _buttonsInfoBlock;

        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _playButton.onClick.AddListener(StartGame);
            _settingsButton.onClick.AddListener(OpenSettings);
            _quitButton.onClick.AddListener(Quit);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _playButton.onClick.RemoveListener(StartGame);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            _quitButton.onClick.RemoveListener(Quit);
        }

        private async void StartGame()
        {
            if (_listOfAllScenes != null)
            {
                await _sceneLoader.LoadSceneAsync(_listOfAllScenes.gameScene);
            }
            else
            {
                int sceneIndex = 1;
                SceneManager.LoadScene(sceneIndex);
            }

            EventsLogManager.LogEvent("PlayButtonClicked");
        }

        private void OpenSettings()
        {
            _settingsMenu.Enable();
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}