using DG.Tweening;
using DI;
using Helpers;
using Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvases
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class CanvasWindowBase : MonoBehaviour, IDIDependent, IUIWindow, IInitializable
    {
        public static Action<CanvasWindowBase> onAWindowOpen { get; private set; }

        [SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

        protected virtual void Awake()
        {
            if (_baseSettings.canvasGroup == null) _baseSettings.canvasGroup = GetComponent<CanvasGroup>();

            onAWindowOpen += OnAWindowOpen;

            TurnOff(0);
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeFromEvents();
            onAWindowOpen -= OnAWindowOpen;

            _baseSettings.canvasGroup.DOKill();
        }

        public virtual void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public virtual void OnAWindowOpen(IUIWindow uIWIndow)
        {
            if (uIWIndow != this as IUIWindow)
            {
                Disable();
            }
        }

        public virtual void Exit()
        {
            Disable();
            _baseSettings.openOnExit?.Enable();
        }

        public virtual void Enable(float? duration = null)
        {
            TurnOn(duration);
        }

        public virtual void Disable(float? duration = null)
        {
            TurnOff(duration);
        }

        protected void TurnOn(float? duration = null)
        {
            if (_baseSettings.isOpen == true) return;
            _baseSettings.isOpen = true;

            if (_baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);

            _baseSettings.canvasGroup.DOKill();

            if (duration == null) duration = _baseSettings.animationDuration_Enable;

            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            _baseSettings.canvasGroup.FadeUp(duration.Value);
        }

        protected void TurnOff(float? duration = null)
        {
            if (_baseSettings.isOpen == false) return;
            _baseSettings.isOpen = false;

            _baseSettings.canvasGroup.DOKill();

            if (duration == null) duration = _baseSettings.animationDuration_Disable;

            if (duration.Value == 0)
            {
                _baseSettings.canvasGroup.FadeDownQuick();
                OnClosed();
            }
            else
            {
                _baseSettings.canvasGroup.FadeDown(duration.Value)?.OnComplete(() =>
                {
                    OnClosed();
                });
            }

            void OnClosed()
            {
                if (_baseSettings.enableDisable) { gameObject.SetActive(false); }
                if (_baseSettings.openOnExit != null && duration.Value != 0) { _baseSettings.openOnExit.Enable(); }
            }
        }

        protected virtual void SubscribeToEvents()
        {
            UnsubscribeFromEvents();
        }

        protected virtual void UnsubscribeFromEvents()
        {

        }

        [Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration_Enable = 1;
            public float animationDuration_Disable = 0.25f;

            [Header("Components")]
            public bool isOpen = true;

            [Header("Components")]
            public CanvasGroup canvasGroup;

            [Header("Optional")]
            public CanvasWindowBase openOnExit;
        }
    }
}