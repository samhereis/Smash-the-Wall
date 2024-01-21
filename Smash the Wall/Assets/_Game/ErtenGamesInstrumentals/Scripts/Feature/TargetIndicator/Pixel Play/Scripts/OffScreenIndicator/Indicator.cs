using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TargetIndicator
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] private IndicatorType _indicatorType;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private Image _distanceTextHolder;
        [SerializeField] private TextMeshProUGUI _distanceText;

        public bool Active
        {
            get
            {
                return transform.gameObject.activeInHierarchy;
            }
        }

        public IndicatorType Type
        {
            get
            {
                return _indicatorType;
            }
        }

        private void Awake()
        {
            if (_indicatorImage == null) _indicatorImage = transform.GetComponent<Image>();
            if (_distanceText == null) _distanceText = transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetImageColor(Color color)
        {
            if (_indicatorImage.color != color) { _indicatorImage.color = color; }
            if (_distanceText.color != color) { _distanceText.color = color; }
        }

        public void SetDistanceText(float value)
        {
            _distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
        }

        public void SetRotation(Quaternion rotation)
        {
            _indicatorImage.rectTransform.rotation = rotation;
        }

        public void Activate(bool value)
        {
            transform.gameObject.SetActive(value);
        }
    }

    public enum IndicatorType
    {
        BOX,
        ARROW
    }
}