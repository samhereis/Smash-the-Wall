using Configs;
using DG.Tweening;
using DI;
using ECS.Systems.GameState;
using Helpers;
using Identifiers;
using InGameStrings;
using Managers;
using PlayerInputHolder;
using TMPro;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayMenu : CanvasWindowBase, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _inputs;
        [DI(DIStrings.gameConfigs)] private GameConfigs _gameConfigs;
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        [Header("UI Components")]
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private ShopWindow _shopWindow;

        [Space(10)]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _shopsButton;

        [Space(10)]
        [SerializeField] private Slider _whatNeedsToBeDestroyedProgressbar;
        [SerializeField] private Image _whatNeedsToBeDestroyedProgressbarFillImage;

        [Space(10)]
        [SerializeField] private Slider _whatNeedsToStayProgressbar;
        [SerializeField] private Image _whatNeedsToStayProgressbarFillImage;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _currentLevelText;

        [Header("Settings")]
        [SerializeField] private Gradient _whatNeedsToBeDestroyedProgressbarGradient;
        [SerializeField] private Gradient _whatNeedsToStayProgressbarGradient;

        private async void Start()
        {
            (this as IDIDependent).LoadDependencies();

            Disable(0);

            await AsyncHelper.Delay(1000);

            Enable();

            _whatNeedsToBeDestroyedProgressbarFillImage = _whatNeedsToBeDestroyedProgressbar.fillRect.GetComponent<Image>();
            _whatNeedsToStayProgressbarFillImage = _whatNeedsToStayProgressbar.fillRect.GetComponent<Image>();

            var _whatNeedsToBeDestroyedProgressbarGradientKeys = _whatNeedsToBeDestroyedProgressbarGradient.colorKeys;
            var _whatNeedsToStayProgressbarGradientKeys = _whatNeedsToStayProgressbarGradient.colorKeys;

            _whatNeedsToBeDestroyedProgressbarGradientKeys[1].time = _gameConfigs.gameplaySettings.percentageOfReleasedWhatNeedsToBeDestroysToWin / 100;
            _whatNeedsToStayProgressbarGradientKeys[0].time = _gameConfigs.gameplaySettings.percentageOfReleasedWhatNeedsToStaysToLose / 100;

            _whatNeedsToBeDestroyedProgressbarGradient.SetKeys(_whatNeedsToBeDestroyedProgressbarGradientKeys, _whatNeedsToBeDestroyedProgressbarGradient.alphaKeys);
            _whatNeedsToStayProgressbarGradient.SetKeys(_whatNeedsToStayProgressbarGradientKeys, _whatNeedsToStayProgressbarGradient.alphaKeys);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _whatNeedsToBeDestroyedProgressbarFillImage?.DOKill();
            _whatNeedsToStayProgressbarFillImage?.DOKill();
        }

        private void Update()
        {
            if (_whatNeedsToBeDestroyedProgressbar != null && _whatNeedsToStayProgressbar != null)
            {
                var releasedWhatNeedsToBeDestroysPercentage = WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage;
                var releasedWhatNeedsToStaysPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

                if (_whatNeedsToBeDestroyedProgressbar.value != releasedWhatNeedsToBeDestroysPercentage)
                {
                    _whatNeedsToBeDestroyedProgressbar?.DOValue(releasedWhatNeedsToBeDestroysPercentage, 0.1f);

                    var color = _whatNeedsToBeDestroyedProgressbarGradient.Evaluate(releasedWhatNeedsToBeDestroysPercentage / _whatNeedsToBeDestroyedProgressbar.maxValue);
                    _whatNeedsToBeDestroyedProgressbarFillImage?.DOColor(color, 0.1f);
                }

                if (_whatNeedsToStayProgressbar.value != releasedWhatNeedsToStaysPercentage)
                {
                    _whatNeedsToStayProgressbar?.DOValue(releasedWhatNeedsToStaysPercentage, 0.1f);

                    var color = _whatNeedsToStayProgressbarGradient.Evaluate(releasedWhatNeedsToStaysPercentage / _whatNeedsToStayProgressbar.maxValue);
                    _whatNeedsToStayProgressbarFillImage?.DOColor(color, 0.1f);
                }
            }
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
                    _adsShowManager?.ShowBanner();
                }
            }
            else
            {
                _adsShowManager?.ShowBanner();
            }

            if (_currentLevelText != null)
            {
                _currentLevelText.text = "current level: " + GameSaveManager.GetLevelIndex();
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
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _pauseButton.onClick.RemoveListener(OpenPauseMenu);
            _shopsButton.onClick.RemoveListener(OnShopsButtonClicked);
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