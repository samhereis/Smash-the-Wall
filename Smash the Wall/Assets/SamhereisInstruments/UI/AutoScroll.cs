using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class AutoScroll : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [Header("Components")]
        [SerializeField] private Scrollbar _scrollBar;

        [SerializeField] private float _startDelay = 3;
        [SerializeField][Range(0.003f, 0.07f)] private float _updateValue = 0.005f;

        [Header("Debug")]
        [SerializeField] private bool _isScrolling = false;

        public void OnPointerUp(PointerEventData eventData)
        {
            StartAutoscroll(_startDelay);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAutoscroll();
        }

        public void Scroll(float value)
        {
            _scrollBar.value += value;
        }

        private void Update()
        {
            if (_isScrolling == true)
            {
                Scroll(-_updateValue);

                if (_scrollBar.value <= 0) _scrollBar.value = 1;
            }
        }

        public void StopAutoscroll()
        {
            StopAllCoroutines();
            _isScrolling = false;
        }

        public async void StartAutoscroll(float delay = 0)
        {
            await AsyncHelper.Delay(delay);
            _isScrolling = true;
        }
    }
}