using Configs;
using DI;
using InGameStrings;
using SO.Lists;
using Tools;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : CanvasWindowBase, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.sceneLoader)][SerializeField] private SceneLoader _sceneLoader;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes; 
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;

        [Header("Components")]
        [SerializeField] private CanvasWindowBase _settingsWindow;
        [SerializeField] private CanvasWindowBase _gameplayWindow;

        [Space(10)]
        [SerializeField] private Image _buttonsInfoBlock;

        [Space(10)]

        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        protected void Start()
        {
            Disable(0);

            (this as IDIDependent).LoadDependencies();
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _buttonsInfoBlock.color = _uIConfigs.uiBackgroundColor_Standart;
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        override protected void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _resumeButton.onClick.AddListener(Resume);
            _settingsButton.onClick.AddListener(OpenSettings);
            _mainMenuButton.onClick.AddListener(OpenMainMenu);
        }

        override protected void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _resumeButton.onClick.RemoveListener(Resume);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            _mainMenuButton.onClick.RemoveListener(OpenMainMenu);
        }

        private void Resume()
        {
            _gameplayWindow?.Enable();
        }

        private void OpenSettings()
        {
            _settingsWindow?.Enable();
        }

        private async void OpenMainMenu()
        {
            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }
    }
}