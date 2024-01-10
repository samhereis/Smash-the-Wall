using Configs;
using DataClasses;
using DependencyInjection;
using ECS.Systems.GameState;
using Helpers;
using Sirenix.OdinInspector;
using SO.Lists;
using Sound;
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
        [SerializeField] private SimpleSound _winAudio;

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

        private PictureMode _currentPictureMode;

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

            switch (_currentPictureMode)
            {
                case Enums.PictureMode.DestroyBorder:
                    {
                        _starControl.gameObject.SetActive(true);

                        _starControl.SetStarCount(_gameConfigs.gameSettings.winLoseStarSettings.Count);
                        _starControl.SetActiveStars(CalculateStars());

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

            SoundPlayer.instance?.TryPlay(_winAudio);
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

        public int CalculateStars()
        {
            int stars = 0;
            float currentPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

            for (int i = 0; i < _gameConfigs.gameSettings.winLoseStarSettings.Count; i++)
            {
                float percentage = _gameConfigs.gameSettings.winLoseStarSettings[i].percentage;

                if (currentPercentage >= percentage)
                {
                    stars = i + 1;
                }
            }

            return stars;
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