using Helpers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AutoScroll : MonoBehaviour, ISelfValidator
    {
        [Required]
        [FoldoutGroup("Components"), ShowInInspector] private Scrollbar _scrollBar;

        [FoldoutGroup("Settings"), ShowInInspector] private float _startDelay = 3;
        [FoldoutGroup("Settings"), ShowInInspector] private float _scrollSpeed = 1;

        [FoldoutGroup("Debug"), ShowInInspector] private bool _isScrolling = false;
        [FoldoutGroup("Debug"), ShowInInspector] private bool _isReverse = false;

        public void Validate(SelfValidationResult result)
        {
            if (_scrollBar == null)
            {
                result.Add(ValidatorSeverity.Warning, "Scrollbar is null").WithFix(() =>
                {
                    Validate();
                    this.TrySetDirty();
                });
            }
        }

        private void Validate()
        {
            if (_scrollBar == null) { _scrollBar = GetComponentInChildren<Scrollbar>(); }
        }

        private void OnEnable()
        {
            Validate();

            _scrollBar.value = 0;
            _isReverse = true;

            StartAutoscroll(_startDelay);
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

        public void Scroll(float value)
        {
            _scrollBar.value += value * Time.deltaTime;
        }

        public async void StartAutoscroll(float delay = 0)
        {
            await AsyncHelper.DelayFloat(delay);
            _isScrolling = true;
        }
    }
}