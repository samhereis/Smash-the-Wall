#if DoTweenInstalled
using DG.Tweening;
#endif

using Sirenix.OdinInspector;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    public class DoFillAmountUpToPoint : MonoBehaviour
    {
        private enum Origin { Left, Right }

        [FoldoutGroup("Settings"), ShowInInspector] private Image _filledImage;
        [FoldoutGroup("Settings"), ShowInInspector] private Image _fillToThisPoint;
        [FoldoutGroup("Settings"), ShowInInspector] private RectTransform _borderHolder;
        [FoldoutGroup("Settings"), ShowInInspector] private Origin _origin;

        [FoldoutGroup("Debug"), ShowInInspector] private float _percent;
        [FoldoutGroup("Debug"), ShowInInspector] private Vector3 _wordPos;
        [FoldoutGroup("Debug"), ShowInInspector] private float _value;
        [FoldoutGroup("Debug"), ShowInInspector] private int _screenWidth;
        [FoldoutGroup("Debug"), ShowInInspector] private Camera _camera_;

        private Camera _camera => _camera_ != null ? _camera_ : _camera_ = FindFirstObjectByType<Camera>(FindObjectsInactive.Include);

        public void Awake()
        {
            _filledImage.fillAmount = 1;

            Close(0);

            _screenWidth = Screen.width;
        }

        private async void OnEnable()
        {
            await AsyncHelper.NextFrame();

#if DoTweenInstalled
            _fillToThisPoint.rectTransform.DOAnchorPosX(0, 1);
#endif
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

        public void Close(float duration)
        {
#if DoTweenInstalled
            if (_origin == Origin.Right) _fillToThisPoint.rectTransform.DOAnchorPosX(-_borderHolder.sizeDelta.x / 2, duration);
            else _fillToThisPoint.rectTransform.DOAnchorPosX(_borderHolder.sizeDelta.x / 2, duration);
#endif
        }

        [Button]
        public void Test()
        {
            Do();
        }
    }
}