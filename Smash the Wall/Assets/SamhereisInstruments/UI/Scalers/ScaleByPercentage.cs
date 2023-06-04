using Helpers;
using LazyUpdators;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Helpers
{
    [ExecuteAlways]
    public class ScaleByPercentage : MonoBehaviour
    {
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _top;
        [SerializeField] private float _bottom;
        [SerializeField] private float _left;
        [SerializeField] private float _right;

        private void OnValidate()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
                this.TrySetDirty();
            }
            if (transform.parent != null)
            {
                _parent = transform.parent.GetComponent<RectTransform>();
                this.TrySetDirty();
            }

            this.TrySetDirty();
        }

        private void Awake()
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //DoAutoScale();
        }

        private async Task DoAutoScale()
        {
            await AsyncHelper.Delay();

            if (_rectTransform != null)
            {
                if (_parent != null)
                {
                    _rectTransform.SetBottom(NumberHelper.GetNumberFromPercentage100(_parent.rect.height, _bottom));
                    _rectTransform.SetTop(NumberHelper.GetNumberFromPercentage100(_parent.rect.height, _top));

                    _rectTransform.SetLeft(NumberHelper.GetNumberFromPercentage100(_parent.rect.width, _left));
                    _rectTransform.SetRight(NumberHelper.GetNumberFromPercentage100(_parent.rect.width, _right));
                }
                else
                {
                    _rectTransform.SetBottom(NumberHelper.GetNumberFromPercentage100(Screen.height, _bottom));
                    _rectTransform.SetTop(NumberHelper.GetNumberFromPercentage100(Screen.height, _top));

                    _rectTransform.SetLeft(NumberHelper.GetNumberFromPercentage100(Screen.width, _left));
                    _rectTransform.SetRight(NumberHelper.GetNumberFromPercentage100(Screen.width, _right));
                }
            }
        }
    }
}