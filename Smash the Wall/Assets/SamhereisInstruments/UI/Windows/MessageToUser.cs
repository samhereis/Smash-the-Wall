using DG.Tweening;
using Helpers;
using TMPro;
using UI.Canvases;
using UnityEngine;

namespace UI
{
    public sealed class MessageToUser : CanvasWindowBase
    {
        public static MessageToUser instance;

        [Header("Log")]
        [SerializeField] private RectTransform _transform;
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Log Error")]
        [SerializeField] private CanvasGroup _logErrorCanvas;
        [SerializeField] private TextMeshProUGUI _logErrorText;

        [Header("Settings")]
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _duration = 2;

        [Header("Log Settings")]
        [SerializeField] private float _showYPosition = 200f;
        [SerializeField] private float _hideYPosition = 200f;
        [SerializeField] private Ease _ease = Ease.InOutBack;

        [Header("Debug")]
        [SerializeField] private bool _isShowingMessage = false;

        protected override void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            base.Awake();
            Hide(0);
        }

        public async void Log(string message)
        {
            await AsyncHelper.Delay();

            ShowUpLog();
            _text.text = message;
        }

        public async void LogError(string message)
        {
            await AsyncHelper.Delay();

            ShowUpError();
            _logErrorText.text = message;
        }

        private void ShowUpLog()
        {
            if (_isShowingMessage == false)
            {
                _isShowingMessage = true;

                _transform.DOAnchorPos3DY(_showYPosition, _animationDuration).SetEase(_ease).OnComplete(async () =>
                {
                    await AsyncHelper.Delay(_duration);
                    Hide(_animationDuration);

                    _isShowingMessage = false;
                });
            }
        }

        private void ShowUpError()
        {
            _transform.DOKill();
            _logErrorCanvas.FadeUp(_animationDuration).SetEase(_ease).OnComplete(async () =>
            {
                await AsyncHelper.Delay(_duration);
                Hide(_animationDuration);
            });
        }

        private void Hide(float duration)
        {
            _transform.DOKill();
            _logErrorCanvas.FadeDown(duration).SetEase(_ease);
            _transform.DOAnchorPos3DY(_hideYPosition, duration).SetEase(_ease);
        }

        [ContextMenu("Debug")]
        public void Debug()
        {
            Log("Test text");
        }
    }
}