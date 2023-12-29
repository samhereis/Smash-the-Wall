using DependencyInjection;
using DG.Tweening;
using Helpers;
using Managers;
using Services;
using Servies;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SO.Lists;
using System.Collections.Generic;
using TMPro;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : CanvasWindowBase
    {
        private const string _isFirstTimeInGameStrings = "IsFirstTimeInGame";
        private const string _adsTrackingOnString = "adsTrackingOnString";
        private const string _analyticsOnString = "analyticsOn";
        private const string _falseString = "false";
        private const string _trueStrings = "true";

        // ---

        private bool _isFirstTimeInGame => PlayerPrefs.GetString(_isFirstTimeInGameStrings, _trueStrings) == _trueStrings;
        private bool _isAdsConsentOn => PlayerPrefs.GetString(_adsTrackingOnString, _trueStrings) == _trueStrings;
        private bool _isAnalyticsSendingOn => PlayerPrefs.GetString(_analyticsOnString, _trueStrings) == _trueStrings;

        // ---

        [FoldoutGroup("Depencencies")]
        [SerializeField] private List<Sprite> _backgroundSprites = new List<Sprite>();

        [Inject]
        [FoldoutGroup("Depencencies/Injected")]
        [SerializeField, ReadOnly] private AdsManager _adsManager;

        [Inject]
        [FoldoutGroup("Depencencies/Injected")]
        [SerializeField, ReadOnly] private EventsLogManager _eventsLogManager;

        [Inject]
        [FoldoutGroup("Depencencies/Injected")]
        [SerializeField, ReadOnly] private SceneLoader _sceneLoader;

        [Inject]
        [FoldoutGroup("Depencencies/Injected")]
        [SerializeField, ReadOnly] private ListOfAllScenes _listOfAllScenes;

        // ---

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private Toggle _adsTrackingConsent;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private Toggle _analyticsConsent;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private Button _startButton;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private Transform _buttonsHolder;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private Image _backgroundImage;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        [Required]
        [FoldoutGroup("Components")]
        [SerializeField] private TextMeshProUGUI _label;

        // ---

        [Header("Settings")]
        [Required]
        [SerializeField] private string _labelAfterInit;

        public override void Validate(SelfValidationResult result)
        {
            base.Validate(result);

            if (_backgroundSprites.IsNullOrEmpty())
            {
                result.AddWarning("Background sprites list is empty");
            }
        }

        protected override void Awake()
        {
            ResetTimeScale();

            base.Awake();

            _buttonsHolder.gameObject.SetActive(false);

            Enable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Update()
        {
            _gridLayoutGroup.cellSize = new Vector2(Screen.height, Screen.height);
        }

        public override async void Enable(float? duration = null)
        {
            base.Enable(duration);

            while (DependencyInjector.isGloballyInjected == false)
            {
                await AsyncHelper.Skip();
            }

            Initialize();

            _buttonsHolder.gameObject.SetActive(false);

            if (_isFirstTimeInGame)
            {
                _label.transform.DOScale(0, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _label.text = _labelAfterInit;
                    _label.transform.DOScale(1, 0.25f);
                });

                try
                {
                    _backgroundSprites.RemoveNulls();
                    _backgroundImage.sprite = _backgroundSprites.GetRandom();
                }
                finally
                {
                    _buttonsHolder.transform.localScale = Vector3.zero;
                    _buttonsHolder.gameObject.SetActive(true);
                    _buttonsHolder.transform.DOScale(1, 1);

                    _startButton.onClick.AddListener(InitializeAndStartGame);
                }
            }
            else
            {
                StartGame();
            }
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            _startButton.onClick.RemoveListener(InitializeAndStartGame);
        }

        private void InitializeAndStartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            PlayerPrefs.SetString(_adsTrackingOnString, _adsTrackingConsent.isOn == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_analyticsOnString, _analyticsConsent.isOn == true ? _trueStrings : _falseString);
            PlayerPrefs.SetString(_isFirstTimeInGameStrings, _falseString);

            StartGame();
        }

        private async void StartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            _adsManager.SetConsent(_isAdsConsentOn);
            _eventsLogManager.SetDataCollectionStatus(_isAnalyticsSendingOn);

            await _sceneLoader.LoadSceneAsync(_listOfAllScenes.mainMenuScene);
        }

        private void ResetTimeScale()
        {
            Time.timeScale = 1;
        }
    }
}