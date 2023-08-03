using DI;
using Helpers;
using LazyUpdators;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Helpers
{
    [ExecuteAlways]
    public class ScaleByPercentage : MonoBehaviour, IDIDependent
    {
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private ScaleSettings _scaleSettings = new ScaleSettings();
        [SerializeField] private IgnoreSettings _ignoreSettings = new IgnoreSettings();

        [Header("DI")]
        [DI(InGameStrings.DIStrings.lazyUpdator)][SerializeField] private LazyUpdator_SO _lazyUpdator;

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
        }

        private void Awake()
        {
            if (Application.isPlaying == true)
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            }
        }

        private void Start()
        {
            if (Application.isPlaying == true)
            {
                (this as IDIDependent)?.LoadDependencies();
            }
        }

        private void OnEnable()
        {
            if (Application.isPlaying == true)
            {
                StartCoroutine(AutoScaleAfterOnEnable());
                _lazyUpdator?.AddToQueue(DoAutoScaleAsync);
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying == true)
            {
                _lazyUpdator?.RemoveFromQueue(DoAutoScaleAsync);
            }
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (Application.isPlaying == false) DoAutoScale();
        }

#endif

        private IEnumerator AutoScaleAfterOnEnable()
        {
            DoAutoScale();

            for (int i = 0; i < 60; i++)
            {
                yield return new WaitForEndOfFrame();
                DoAutoScale();
            }
        }

        [ContextMenu(nameof(DoAutoScale))]
        public void DoAutoScale()
        {
            if (_rectTransform != null)
            {
                if (_parent != null)
                {
                    if (_ignoreSettings.ignoreTop == false) _rectTransform.SetTop(NumberHelper.GetNumberFromPercentage(_parent.rect.height, _scaleSettings.top));
                    if (_ignoreSettings.ignoreBottom == false) _rectTransform.SetBottom(NumberHelper.GetNumberFromPercentage(_parent.rect.height, _scaleSettings.bottom));

                    if (_ignoreSettings.ignoreRight == false) _rectTransform.SetRight(NumberHelper.GetNumberFromPercentage(_parent.rect.width, _scaleSettings.right));
                    if (_ignoreSettings.ignoreLeft == false) _rectTransform.SetLeft(NumberHelper.GetNumberFromPercentage(_parent.rect.width, _scaleSettings.left));
                }
                else
                {
                    if (_ignoreSettings.ignoreTop == false) _rectTransform.SetTop(NumberHelper.GetNumberFromPercentage(Screen.height, _scaleSettings.top));
                    if (_ignoreSettings.ignoreBottom == false) _rectTransform.SetBottom(NumberHelper.GetNumberFromPercentage(Screen.height, _scaleSettings.bottom));

                    if (_ignoreSettings.ignoreRight == false) _rectTransform.SetRight(NumberHelper.GetNumberFromPercentage(Screen.width, _scaleSettings.right));
                    if (_ignoreSettings.ignoreLeft == false) _rectTransform.SetLeft(NumberHelper.GetNumberFromPercentage(Screen.width, _scaleSettings.left));
                }
            }
        }

        private async Task DoAutoScaleAsync()
        {
            DoAutoScale();
            await AsyncHelper.Delay();
        }

        [Serializable]
        public class ScaleSettings
        {
            [field: SerializeField] public float top { get; private set; } = 0;
            [field: SerializeField] public float bottom { get; private set; } = 0;
            [field: SerializeField] public float left { get; private set; } = 0;
            [field: SerializeField] public float right { get; private set; } = 0;
        }

        [Serializable]
        public class IgnoreSettings
        {
            [field: SerializeField] public bool ignoreTop { get; private set; } = false;
            [field: SerializeField] public bool ignoreBottom { get; private set; } = false;
            [field: SerializeField] public bool ignoreLeft { get; private set; } = false;
            [field: SerializeField] public bool ignoreRight { get; private set; } = false;
        }
    }
}