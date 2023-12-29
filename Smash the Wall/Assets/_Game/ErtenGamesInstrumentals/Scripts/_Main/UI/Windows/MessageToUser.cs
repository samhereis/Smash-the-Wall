#if DoTweenInstalled
using DG.Tweening;
#endif

using Helpers;
using Sirenix.OdinInspector;
using TMPro;
using UI.Canvases;
using UnityEngine;

namespace UI
{
    public sealed class MessageToUser : CanvasWindowBase
    {
        public static MessageToUser instance;

        [Header("Log")]

        [Required]
        [SerializeField] private RectTransform _transform;

        [Required]
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Log Error")]

        [Required]
        [SerializeField] private CanvasGroup _logErrorCanvas;

        [Required]
        [SerializeField] private TextMeshProUGUI _logErrorText;

        [Header("Settings")]
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _duration = 2;

        [Header("Log Settings")]
        [SerializeField] private float _showYPosition = 200f;
        [SerializeField] private float _hideYPosition = 200f;

#if DoTweenInstalled
        [SerializeField] private Ease _ease = Ease.InOutBack;
#endif

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
            await AsyncHelper.Skip();

            ShowUpLog();
            _text.text = message;
        }

        public async void LogError(string message)
        {
            await AsyncHelper.Skip();

            ShowUpError();
            _logErrorText.text = message;
        }

        private void ShowUpLog()
        {
            if (_isShowingMessage == false)
            {
                _isShowingMessage = true;

#if DoTweenInstalled
                _transform.DOAnchorPos3DY(_showYPosition, _animationDuration).SetEase(_ease).OnComplete(async () =>
                {
                    await AsyncHelper.DelayFloat(_duration);
                    Hide(_animationDuration);

                    _isShowingMessage = false;
                });
#endif
            }
        }

        private void ShowUpError()
        {
#if DoTweenInstalled
            _transform.DOKill();
            _logErrorCanvas.FadeUp(_animationDuration).SetEase(_ease).OnComplete(async () =>
            {
                await AsyncHelper.DelayFloat(_duration);
                Hide(_animationDuration);
            });
#endif
        }

        private void Hide(float duration)
        {
#if DoTweenInstalled
            _transform.DOKill();
            _logErrorCanvas.FadeDown(duration).SetEase(_ease);
            _transform.DOAnchorPos3DY(_hideYPosition, duration).SetEase(_ease);
#endif
        }

        [ContextMenu("Debug")]
        public void Debug()
        {
            Log("Test text");
        }
    }
}