using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Canvases
{
    public abstract class CanvasWindowBase : MonoBehaviour
    {
        protected static Action<CanvasWindowBase> onAWindowOpen;
        [SerializeField] protected BaseSettings baseSettings = new BaseSettings();

        private TweenerCore<float, float, FloatOptions> _currentTween;

        protected virtual void Awake()
        {
            baseSettings.SetCanvas(this);
            onAWindowOpen += OnAWindowOpen;
        }

        protected virtual void Start()
        {
            FindAllUIElements();
        }

        protected virtual void OnDestroy()
        {
            onAWindowOpen -= OnAWindowOpen;
        }

        private void Update()
        {
            baseSettings.animatedVisualElementsDebug.Clear();
            baseSettings.animatedVisualElements.RemoveNulls();

            foreach (var item in baseSettings.animatedVisualElements)
            {
                baseSettings.animatedVisualElementsDebug.Add(item.name);
            }
        }

        public virtual void OnAWindowOpen(CanvasWindowBase uIWIndow)
        {
            if (uIWIndow != this) Disable();
        }

        protected virtual void FindAllUIElements()
        {

        }

        protected virtual void SubscribeToUIEvents()
        {
            UnSubscribeFromUIEvents();
        }

        protected virtual void UnSubscribeFromUIEvents()
        {

        }

        [ContextMenu(nameof(Open))]
        public void Open()
        {
            Enable();
        }

        [ContextMenu(nameof(Close))]
        public void Close()
        {
            Disable();
        }

        public virtual async void Enable(float? duration = null)
        {
            if (baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);
            if (duration == null) duration = baseSettings.animationDuration;

            baseSettings.SetVisibility(true);

            await AsyncHelper.Delay(baseSettings.enableAnimationDelay);

            _currentTween = baseSettings.root.style.opacity.value.TweenFloat(1, duration.Value, onUpdateCallback: (currentValue) =>
            {
                baseSettings.ChangeOpasity(currentValue);
            });

            PlayShowAnimation();
            SubscribeToUIEvents();
        }

        public virtual async void Disable(float? duration = null)
        {
            if (duration == null) duration = baseSettings.animationDuration;

            await AsyncHelper.Delay(baseSettings.disableAnimationDelay);

            _currentTween = baseSettings.root.style.opacity.value.TweenFloat(0, duration.Value, onUpdateCallback: (currentValue) =>
            {
                baseSettings.ChangeOpasity(currentValue);
            },
            completedCallback: (value) =>
            {
                if (baseSettings.enableDisable) baseSettings.SetVisibility(false);
            });

            PlayHideAnimation();
            UnSubscribeFromUIEvents();
        }

        [ContextMenu(nameof(PlayShowAnimation))]
        protected void PlayShowAnimation()
        {
            baseSettings.animatedVisualElements.RemoveNulls();

            foreach (var element in baseSettings.animatedVisualElements)
            {
                element.SetEnabled(true);
            }
        }

        [ContextMenu(nameof(PlayHideAnimation))]
        protected void PlayHideAnimation()
        {
            baseSettings.animatedVisualElements.RemoveNulls();

            foreach (var element in baseSettings.animatedVisualElements)
            {
                element.SetEnabled(false);
            }
        }

        [System.Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration = 0.5f;
            public int enableAnimationDelay = 0;
            public int disableAnimationDelay = 0;

            [Header("Components")]
            public UIDocument uIDocument;

            public List<VisualElement> animatedVisualElements = new List<VisualElement>();
            public List<string> animatedVisualElementsDebug = new List<string>();

            private CanvasWindowBase _canvas;

            public VisualElement root => uIDocument != null ? uIDocument.rootVisualElement : (uIDocument = _canvas.GetComponent<UIDocument>()).rootVisualElement;

            public void SetCanvas(CanvasWindowBase canvas)
            {
                _canvas = canvas;
                uIDocument = _canvas.GetComponent<UIDocument>();
            }

            public void ChangeOpasity(float opacity)
            {
                if (root != null) root.style.opacity = new StyleFloat(opacity);
            }

            public void SetVisibility(bool enabled)
            {
                if (root != null)
                {
                    root.visible = enabled;
                }
            }
        }
    }
}