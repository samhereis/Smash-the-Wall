using Configs;
using DependencyInjection;
using DG.Tweening;
using ECS.Systems.GameState;
using Helpers;
using Identifiers;
using Managers;
using Services;
using Sirenix.OdinInspector;
using SO.Lists;
using System.Threading.Tasks;
using TMPro;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayMenu : CanvasWindowBase
    {
        [Header("Dependency")]
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private ShopWindow _shopWindow;

        [Header("DI")]
        [Inject][SerializeField] private InputsService _inputs;
        [Inject][SerializeField] private GameConfigs _gameConfigs;
        [Inject][SerializeField] private AdsShowManager _adsShowManager;
        [Inject][SerializeField] private LazyUpdator_Service _lazyUpdator;
        [Inject][SerializeField] private GameSaveManager _gameSaveManager;
        [Inject][SerializeField] private ListOfAllPictures _listOfAllPictures;

        [Header("UI Components")]

        [Required]
        [SerializeField] private Button _pauseButton;

        [Required]
        [SerializeField] private Button _shopsButton;

        [Required]
        [SerializeField] private Slider _whatNeedsToBeDestroyedProgressbar;

        [Required]
        [SerializeField] private Slider _whatNeedsToStayProgressbar;

        [Required]
        [SerializeField] private TextMeshProUGUI _currentLevelText;

        [Header("Settings")]
        [SerializeField] private Gradient _whatNeedsToBeDestroyedProgressbarGradient;
        [SerializeField] private Gradient _whatNeedsToStayProgressbarGradient;

        private Image _whatNeedsToBeDestroyedProgressbarFillImage;
        private Image _whatNeedsToStayProgressbarFillImage;

        public override void Initialize()
        {
            base.Initialize();

            if (_pauseMenu == null) { _pauseMenu = FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include); }
            if (_shopWindow == null) { _shopWindow = FindFirstObjectByType<ShopWindow>(FindObjectsInactive.Include); }

            _whatNeedsToBeDestroyedProgressbarFillImage = _whatNeedsToBeDestroyedProgressbar.fillRect.GetComponent<Image>();
            var _whatNeedsToBeDestroyedProgressbarGradientKeys = _whatNeedsToBeDestroyedProgressbarGradient.colorKeys;
            _whatNeedsToBeDestroyedProgressbarGradientKeys[1].time = _gameConfigs.gameSettings.percentageOfReleasedWhatNeedsToBeDestroysToWin / 100;
            _whatNeedsToBeDestroyedProgressbarGradient.SetKeys(_whatNeedsToBeDestroyedProgressbarGradientKeys, _whatNeedsToBeDestroyedProgressbarGradient.alphaKeys);

            if (_listOfAllPictures.GetCurrent().pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                _whatNeedsToStayProgressbarFillImage = _whatNeedsToStayProgressbar.fillRect.GetComponent<Image>();
                var _whatNeedsToStayProgressbarGradientKeys = _whatNeedsToStayProgressbarGradient.colorKeys;
                _whatNeedsToStayProgressbarGradientKeys[0].time = _gameConfigs.gameSettings.percentageOfReleasedWhatNeedsToStaysToLose / 100;
                _whatNeedsToStayProgressbarGradient.SetKeys(_whatNeedsToStayProgressbarGradientKeys, _whatNeedsToStayProgressbarGradient.alphaKeys);
            }
            else
            {
                Destroy(_whatNeedsToStayProgressbar.gameObject);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _whatNeedsToBeDestroyedProgressbarFillImage?.DOKill();
            _whatNeedsToStayProgressbarFillImage?.DOKill();

            _lazyUpdator?.RemoveFromQueue(LazyUpdate);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            _inputs.Enable();

            SubscribeToEvents();

            bool shouldDestroyBanner = false;

            var banners = FindObjectsOfType<BannerIdentifier>(true);

            foreach (var item in banners)
            {
                if (item.gameObject.activeInHierarchy == false)
                {
                    Destroy(item.gameObject);

                    if (shouldDestroyBanner == false) { shouldDestroyBanner = true; }
                }
            }

            if (banners.Length > 0)
            {
                if (shouldDestroyBanner == true)
                {
                    _adsShowManager?.DestroyBanner();
                    _adsShowManager?.TryShowBanner();
                }
            }
            else
            {
                _adsShowManager?.TryShowBanner();
            }

            if (_currentLevelText != null)
            {
                _currentLevelText.text = "current level: " + _gameSaveManager.GetLevelIndex();
            }
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            _inputs.Disable();

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _pauseButton.onClick.AddListener(OpenPauseMenu);
            _shopsButton.onClick.AddListener(OnShopsButtonClicked);

            _lazyUpdator?.AddToQueue(LazyUpdate);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _pauseButton.onClick.RemoveListener(OpenPauseMenu);
            _shopsButton.onClick.RemoveListener(OnShopsButtonClicked);

            _lazyUpdator?.RemoveFromQueue(LazyUpdate);
        }

        private async Task LazyUpdate()
        {
            TryUpdatereWhatNeedsToBeDestroyProgressBar();
            TryUpdatereWhatNeedsToStayProgressBar();

            await AsyncHelper.DelayFloat(100);
        }

        private void TryUpdatereWhatNeedsToBeDestroyProgressBar()
        {
            if (_whatNeedsToBeDestroyedProgressbar != null)
            {
                var releasedWhatNeedsToBeDestroysPercentage = WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage;

                if (_whatNeedsToBeDestroyedProgressbar.value != releasedWhatNeedsToBeDestroysPercentage)
                {
                    _whatNeedsToBeDestroyedProgressbar?.DOValue(releasedWhatNeedsToBeDestroysPercentage, 0.1f);

                    var color = _whatNeedsToBeDestroyedProgressbarGradient.Evaluate(releasedWhatNeedsToBeDestroysPercentage / _whatNeedsToBeDestroyedProgressbar.maxValue);
                    _whatNeedsToBeDestroyedProgressbarFillImage?.DOColor(color, 0.1f);
                }
            }
        }

        private void TryUpdatereWhatNeedsToStayProgressBar()
        {
            if (_whatNeedsToStayProgressbar != null && _listOfAllPictures.GetCurrent().pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                var releasedWhatNeedsToStaysPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

                if (_whatNeedsToStayProgressbar.value != releasedWhatNeedsToStaysPercentage)
                {
                    _whatNeedsToStayProgressbar?.DOValue(releasedWhatNeedsToStaysPercentage, 0.1f);

                    var color = _whatNeedsToStayProgressbarGradient.Evaluate(releasedWhatNeedsToStaysPercentage / _whatNeedsToStayProgressbar.maxValue);
                    _whatNeedsToStayProgressbarFillImage?.DOColor(color, 0.1f);
                }
            }
        }

        private void OpenPauseMenu()
        {
            _pauseMenu?.Enable();
        }

        private void OnShopsButtonClicked()
        {
            _shopWindow?.Enable();
        }
    }
}