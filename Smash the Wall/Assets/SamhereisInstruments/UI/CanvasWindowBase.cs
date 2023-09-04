using Configs;
using DG.Tweening;
using DI;
using Helpers;
using InGameStrings;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UI.Interaction;
using UI.UIAnimationElements;
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

        private CancellationTokenSource _onDestroyCTS = new CancellationTokenSource();

#if UNITY_EDITOR

        [SerializeField] private List<Button> _buttons = new List<Button>();

        private void OnValidate()
        {
            foreach (var button in GetComponentsInChildren<Button>(true))
            {
                if (button is not BetterButton)
                {
                    _buttons.SafeAdd(button);
                }
            }

            if (_buttons.Count > 0)
            {
                Debug.Log(gameObject.name + " Has a standart button");
            }
        }

#endif

        protected virtual void Awake()
        {
            if (_baseSettings.canvasGroup == null) _baseSettings.canvasGroup = GetComponent<CanvasGroup>();
            _baseSettings.uIAnimationElements = GetComponentsInChildren<UIAnimationElement_Base>(true);

            onAWindowOpen += OnAWindowOpen;

            TurnOff(0);
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeFromEvents();
            onAWindowOpen -= OnAWindowOpen;

            _baseSettings.canvasGroup.DOKill();

            _onDestroyCTS.Cancel();
        }

        public virtual void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
            _baseSettings.Initialize();
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

            TurnOnUIAnimationElements_Async();

            if (_baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);

            _baseSettings.canvasGroup.DOKill();

            if (duration == null) duration = _baseSettings.animationDuration_Enable;

            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            _baseSettings.canvasGroup.FadeUp(duration.Value);

            async void TurnOnUIAnimationElements_Async()
            {
                float uiAnimationElementForeachDelay = 0.025f;

                if (_baseSettings.uIConfigs != null) uiAnimationElementForeachDelay = _baseSettings.uIConfigs.uiAnimationElementForeachDelay;

                foreach (var uiAnimationElement in _baseSettings.uIAnimationElements)
                {
                    if (_onDestroyCTS.IsCancellationRequested == true) break;

                    try
                    {
                        uiAnimationElement?.TurnOn();

                        await AsyncHelper.Delay(uiAnimationElementForeachDelay);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("Error animating ui animation element: " + ex, gameObject);
                    }
                }
            }
        }

        protected void TurnOff(float? duration = null)
        {
            if (_baseSettings.isOpen == false) return;
            _baseSettings.isOpen = false;

            TurnOffUIAnimationElements();

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

            void TurnOffUIAnimationElements()
            {
                foreach (var uiAnimationElement in _baseSettings.uIAnimationElements)
                {
                    try
                    {
                        uiAnimationElement?.TurnOff();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("Error animating ui animation element: " + ex, gameObject);
                    }
                }
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
        protected class BaseSettings : IDIDependent, IInitializable
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

            [Header("DI")]
            [DI(DIStrings.uiConfigs)] public UIConfigs uIConfigs;

            [Header("Optional")]
            public CanvasWindowBase openOnExit;

            [Header("Debug")]
            public UIAnimationElement_Base[] uIAnimationElements;

            public void Initialize()
            {
                (this as IDIDependent).LoadDependencies();
            }
        }
    }
}