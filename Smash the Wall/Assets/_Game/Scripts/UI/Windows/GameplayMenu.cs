using Configs;
using DependencyInjection;
using DG.Tweening;
using GameState;
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
        [FoldoutGroup("Settings"), SerializeField] private float _progressbarsUpdateRate = 1;

        [Inject] private GameConfigs _gameConfigs;
        [Inject] private GameSaveManager _gameSaveManager;
        [Inject] private ListOfAllPictures _listOfAllPictures;

#if InputSystemInstalled
        [Inject] private Inputs.PlayerInputData _inputs;
#endif

        private ShopMenu _shopWindow;

        private Image _whatNeedsToBeDestroyedProgressbarFillImage;
        private Image _whatNeedsToStayProgressbarFillImage;

        private LazyUpdator_Service _lazyUpdator = new LazyUpdator_Service();

        private Gameplay_GameState_Model _gameplay_GameState_Model;

        public void Initialize(ShopMenu shopWindow,
            Gameplay_GameState_Model gameplay_GameState_Model)
        {
            _shopWindow = shopWindow;
            _gameplay_GameState_Model = gameplay_GameState_Model;

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

            _pauseButton.onClick.AddListener(Pause);
            _shopsButton.onClick.AddListener(OnShopsButtonClicked);

            _lazyUpdator?.AddToQueue(LazyUpdate);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _pauseButton.onClick.RemoveListener(Pause);
            _shopsButton.onClick.RemoveListener(OnShopsButtonClicked);

            _lazyUpdator?.RemoveFromQueue(LazyUpdate);
        }

        private async Task LazyUpdate()
        {
            TryUpdatereWhatNeedsToBeDestroyProgressBar();
            TryUpdatereWhatNeedsToStayProgressBar();

            await AsyncHelper.DelayFloat(1f);
        }

        private void TryUpdatereWhatNeedsToBeDestroyProgressBar()
        {
            if (_whatNeedsToBeDestroyedProgressbar != null)
            {
                if (_whatNeedsToBeDestroyedProgressbar.value != _gameplay_GameState_Model.releasedWhatNeedsToBeDestroysPercentage)
                {
                    _whatNeedsToBeDestroyedProgressbar?.DOValue(_gameplay_GameState_Model.releasedWhatNeedsToBeDestroysPercentage, _progressbarsUpdateRate);

                    var color = _whatNeedsToBeDestroyedProgressbarGradient.Evaluate(_gameplay_GameState_Model.releasedWhatNeedsToBeDestroysPercentage / _whatNeedsToBeDestroyedProgressbar.maxValue);
                    _whatNeedsToBeDestroyedProgressbarFillImage?.DOColor(color, _progressbarsUpdateRate);
                }
            }
        }

        private void TryUpdatereWhatNeedsToStayProgressBar()
        {
            if (_whatNeedsToStayProgressbar != null && _listOfAllPictures.GetCurrent().pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                if (_whatNeedsToStayProgressbar.value != _gameplay_GameState_Model.releasedWhatNeedsToStaysPercentage)
                {
                    _whatNeedsToStayProgressbar?.DOValue(_gameplay_GameState_Model.releasedWhatNeedsToStaysPercentage, _progressbarsUpdateRate);

                    var color = _whatNeedsToStayProgressbarGradient.Evaluate(_gameplay_GameState_Model.releasedWhatNeedsToStaysPercentage / _whatNeedsToStayProgressbar.maxValue);
                    _whatNeedsToStayProgressbarFillImage?.DOColor(color, _progressbarsUpdateRate);
                }
            }
        }

        private void Pause()
        {
            _gameplay_GameState_Model.onGameplayStatusChanged?.Invoke(Gameplay_GameState_Model.GameplayState.Pause);
        }

        private void OnShopsButtonClicked()
        {
            _shopWindow?.Enable();
        }
    }
}