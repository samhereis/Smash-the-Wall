using Configs;
using DataClasses;
using DependencyInjection;
using Helpers;
using Sirenix.OdinInspector;
using SO;
using SO.Lists;
using Sounds;
using System;
using UI.Canvases;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;
using static DataClasses.Enums;

namespace UI
{
    public class WinMenu : MenuBase, INeedDependencyInjection
    {
        public Action onMainMenuClicked;
        public Action onNextClicked;

        [Header("Effects")]

        [Required]
        [SerializeField] private Sprite[] _winEmojis;

        [Required]
        [SerializeField] private Sound_String_SO _winAudio;

        [Header("Components")]

        [Required]
        [SerializeField] private Button _nextLevelButton;

        [Required]
        [SerializeField] private Button _goToMainMenuButton;

        [Required]
        [SerializeField] private Image _winInfoBlock;

        [Required]
        [SerializeField] private Image _buttonsInfoBlock;

        [Required]
        [SerializeField] private Image _winEmoji;

        [Required]
        [SerializeField] private Star_CustomControl _starControl;

        [Inject] private GameConfigs _gameConfigs;
        [Inject] private ListOfAllPictures _listOfAllPictures;
        [Inject] private SoundPlayer _soundPlayer;

        private IWinStarCalculator _winStarCalculator;

        private PictureMode _currentPictureMode;

        public void Initialize(IWinStarCalculator winStarCalculator)
        {
            _winStarCalculator = winStarCalculator;

            Initialize();
        }

        public void Initialize()
        {
            DependencyContext.InjectDependencies(this);

            _currentPictureMode = _listOfAllPictures.GetCurrent().pictureMode;
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _winEmoji.gameObject.SetActive(false);
            _starControl.gameObject.SetActive(false);

            _gameConfigs.isRestart = false;

            _soundPlayer?.TryPlay(_winAudio);

            switch (_currentPictureMode)
            {
                case Enums.PictureMode.DestroyBorder:
                    {
                        _starControl.gameObject.SetActive(true);

                        _starControl.SetStarCount(_gameConfigs.gameSettings.winLoseStarSettings.Count);
                        _starControl.SetActiveStars(_winStarCalculator != null ? _winStarCalculator.CalculateWinStars() : 5, 0.75f);

                        break;
                    }
                case Enums.PictureMode.DestroyWholeObject:
                    {
                        _winEmoji.sprite = _winEmojis.GetRandom();

                        _winEmoji.gameObject.SetActive(true);

                        break;
                    }
                case Enums.PictureMode.Coloring:
                    {
                        break;
                    }
            }
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _nextLevelButton.onClick.AddListener(NextLevel);
            _goToMainMenuButton.onClick.AddListener(GotoMainMenu);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _nextLevelButton.onClick.RemoveListener(NextLevel);
            _goToMainMenuButton.onClick.RemoveListener(GotoMainMenu);
        }

        private void NextLevel()
        {
            onNextClicked?.Invoke();
        }

        private void GotoMainMenu()
        {
            onMainMenuClicked?.Invoke();
        }
    }
}