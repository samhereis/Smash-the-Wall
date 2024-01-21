using DG.Tweening;
using Helpers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : MenuBase
    {
        public Action onStartClicked;

        public bool adsTrackingConsent => _adsTrackingConsent.isOn;
        public bool analyticsConsent => _analyticsConsent.isOn;

        [FoldoutGroup("Depencencies")]
        [SerializeField] private List<Sprite> _backgroundSprites = new List<Sprite>();

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
            base.Awake();

            ResetTimeScale();

            Enable();
        }

        private void Update()
        {
            _gridLayoutGroup.cellSize = new Vector2(Screen.height, Screen.height);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

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
            }

            _startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            _startButton.onClick.RemoveListener(StartGame);

            onStartClicked?.Invoke();
        }

        private void ResetTimeScale()
        {
            Time.timeScale = 1;
        }
    }
}