using Configs;
using DependencyInjection;
using DG.Tweening;
using ECS.Systems.GameState;
using Helpers;
using Identifiers;
using Interfaces;
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
    public class GameplayMenu : MenuBase, IInitializable, INeedDependencyInjection
    {
        [Required, ChildGameObjectsOnly]
        [FoldoutGroup("UI Components"), SerializeField] private Button _pauseButton;

        [Required, ChildGameObjectsOnly]
        [FoldoutGroup("UI Components"), SerializeField] private Button _shopsButton;

        [Required, ChildGameObjectsOnly]
        [FoldoutGroup("UI Components"), SerializeField] private Slider _whatNeedsToBeDestroyedProgressbar;

        [Required, ChildGameObjectsOnly]
        [FoldoutGroup("UI Components"), SerializeField] private Slider _whatNeedsToStayProgressbar;

        [Required, ChildGameObjectsOnly]
        [FoldoutGroup("UI Components"), SerializeField] private TextMeshProUGUI _currentLevelText;

        [FoldoutGroup("Settings"), SerializeField] private Gradient _whatNeedsToBeDestroyedProgressbarGradient;
        [FoldoutGroup("Settings"), SerializeField] private Gradient _whatNeedsToStayProgressbarGradient;

        [FoldoutGroup("Dependency"), SerializeField, ReadOnly] private PauseMenu _pauseMenu;
        [FoldoutGroup("Dependency"), SerializeField, ReadOnly] private ShopMenu _shopWindow;

        [Inject] private GameConfigs _gameConfigs;
        [Inject] private AdsShowManager _adsShowManager;
        [Inject] private GameSaveManager _gameSaveManager;
        [Inject] private ListOfAllPictures _listOfAllPictures;

#if InputSystemInstalled
        [Inject] private InputsService _inputs;
#endif

        private Image _whatNeedsToBeDestroyedProgressbarFillImage;
        private Image _whatNeedsToStayProgressbarFillImage;

        private LazyUpdator_Service _lazyUpdator = new LazyUpdator_Service();

        public void Initialize(PauseMenu pauseMenu, ShopMenu shopWindow)
        {
            _pauseMenu = pauseMenu;
            _shopWindow = shopWindow;

            Initialize();
        }

        public void Initialize()
        {
            DependencyContext.InjectDependencies(this);

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

            UnsubscribeFromEvents();
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

#if InputSystemInstalled
            _inputs.Enable();
#endif
            SubscribeToEvents();

            TryShowBanner();

            if (_currentLevelText != null)
            {
                _currentLevelText.text = "current level: " + _gameSaveManager.GetLevelIndex();
            }
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

#if InputSystemInstalled
            _inputs.Disable();
#endif

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

        private void TryShowBanner()
        {
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
        }
    }
}