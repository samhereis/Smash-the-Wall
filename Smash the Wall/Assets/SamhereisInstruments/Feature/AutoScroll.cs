using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AutoScroll : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Scrollbar _scrollBar;

        [SerializeField] private float _startDelay = 3;
        [SerializeField] private float _scrollSpeed = 1;

        [Header("Debug")]
        [SerializeField] private bool _isScrolling = false;
        [SerializeField] private bool _isReverse = false;

        private void OnEnable()
        {
            _scrollBar.value = 0;
            _isReverse = true;

            StartCoroutine(StartAutoscroll(_startDelay));
        }

        public void Scroll(float value)
        {
            _scrollBar.value += value * Time.deltaTime;
        }

        private void Update()
        {
            if (_isScrolling == true)
            {
                if (_scrollBar.value >= 1)
                {
                    if (_isReverse == true) _isReverse = false; else _isReverse = true;
                }
                else if (_scrollBar.value <= 0)
                {
                    if (_isReverse == false) _isReverse = true; else _isReverse = false;
                }

                Scroll(_scrollSpeed * (_isReverse == true ? -1f : 1f));
            }
        }

        public IEnumerator StartAutoscroll(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            _isScrolling = true;
        }
    }
}