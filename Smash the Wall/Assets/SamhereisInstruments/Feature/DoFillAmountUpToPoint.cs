using DG.Tweening;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class DoFillAmountUpToPoint : MonoBehaviour
    {
        private enum Origin { Left, Right }

        [SerializeField] private Image _filledImage;
        [SerializeField] private Image _fillToThisPoint;
        [SerializeField] private RectTransform _borderHolder;
        [SerializeField] private Origin _origin;

        [Header("Debug")]
        [SerializeField] private float _percent;
        [SerializeField] private Vector3 _wordPos;
        [SerializeField] private float _value;
        [SerializeField] private int _screenWidth;
        [SerializeField] private Camera _cam;

        private Camera _camera => _cam != null ? _cam : _cam = FindObjectOfType<Camera>();

        public void Awake()
        {
            _filledImage.fillAmount = 1;

            Close(0);

            _screenWidth = Screen.width;
        }

        public void Close(float duration)
        {
            if (_origin == Origin.Right) _fillToThisPoint.rectTransform.DOAnchorPosX(-_borderHolder.sizeDelta.x / 2, duration);
            else _fillToThisPoint.rectTransform.DOAnchorPosX(_borderHolder.sizeDelta.x / 2, duration);
        }

        private async void OnEnable()
        {
            await AsyncHelper.Delay(500);

            _fillToThisPoint.rectTransform.DOAnchorPosX(0, 1);
        }

        private void Update()
        {
            Do();
        }

        private void Do()
        {
            _wordPos = _camera.WorldToScreenPoint(_fillToThisPoint.rectTransform.position);
            _value = _wordPos.x;

            if (_origin == Origin.Right) _value -= _screenWidth;

            _percent = NumberHelper.GetPercentageOf1(_value, _screenWidth);
            _filledImage.fillAmount = Mathf.Abs(_percent);
        }

        [ContextMenu(nameof(Test))]
        public void Test()
        {
            Do();
        }
    }
}