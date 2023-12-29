using DependencyInjection;
using Helpers;
using Managers;
using Servies;
using Sirenix.OdinInspector;
using SO.DataHolders;
using SO.Lists;
using Sound;
using UI.Canvases;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : CanvasWindowBase
    {
        [Header("DI")]
        [Inject][SerializeField] private ListOfAllScenes _listOfAllScenes;
        [Inject][SerializeField] private SceneLoader _sceneLoader;
        [Inject][SerializeField] private BackgroundMusicPlayer _backgroundMusicPlayer;

        [Header("Dependencies")]

        [SceneObjectsOnly]
        [SerializeField] private SettingsMenu _settingsMenu;

        [Header("Components")]

        [Required]
        [SerializeField] private SoundsPack_DataHolder _mainMenuSoundsPack;

        [Required]
        [SerializeField] private Image _buttonsInfoBlock;

        [Header("Buttons")]

        [Required]
        [SerializeField] private Button _playButton;

        [Required]
        [SerializeField] private Button _settingsButton;

        [Required]
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

            PlayGameplayAudio(_backgroundMusicPlayer, _mainMenuSoundsPack);
        }

        public static async void PlayGameplayAudio(BackgroundMusicPlayer backgroundMusicPlayer, SoundsPack_DataHolder soundsPack)
        {
            backgroundMusicPlayer.StopMusic();

            await AsyncHelper.DelayFloat(2f);

            backgroundMusicPlayer.PlayMusic(soundsPack);
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