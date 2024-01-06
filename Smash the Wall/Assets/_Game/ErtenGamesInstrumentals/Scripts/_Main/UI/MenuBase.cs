﻿#if DoTweenInstalled
using DG.Tweening;
#endif

using Helpers;
using Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UI.Helpers;
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
    public abstract class MenuBase : MonoBehaviour, IMenuWindow, ISelfValidator
    {
        public Action onEnable;
        public Action onDisable;

        public Action onSubscribeToEvents;
        public Action onUnsubscribeFromEvents;

        public static Action<MenuBase> onAWindowOpen { get; private set; }

        [FoldoutGroup("BaseSettings")][SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

#if UNITY_EDITOR

        [FoldoutGroup("BaseSettings")][SerializeField] private List<Button> _buttons = new List<Button>();
        [FoldoutGroup("BaseSettings")][SerializeField] protected ScaleByPercentage[] _scaleByPercentages;

#endif

        public virtual void Validate(SelfValidationResult result)
        {
#if UNITY_EDITOR

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

            _scaleByPercentages = GetComponentsInChildren<ScaleByPercentage>(true);

#endif

            if (_baseSettings.canvasGroup == null) _baseSettings.canvasGroup = GetComponent<CanvasGroup>();
            _baseSettings.uIAnimationElements = GetComponentsInChildren<UIAnimationElement_Base>(true);
        }

        protected virtual void Awake()
        {
            onAWindowOpen += OnAWindowOpen;

            TurnOff(0);
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeFromEvents();
            onAWindowOpen -= OnAWindowOpen;

#if DoTweenInstalled
            _baseSettings.canvasGroup.DOKill();
#endif
        }

        public virtual void OnAWindowOpen(IMenuWindow uIWIndow)
        {
            if (uIWIndow != this as IMenuWindow)
            {
                Disable();
            }
        }

        public virtual void Enable(float? duration = null)
        {
            if (_baseSettings.isOpen == true) return;

            TurnOn(duration);

            onEnable?.Invoke();
        }

        public virtual void Disable(float? duration = null)
        {
            if (_baseSettings.isOpen == false) return;

            TurnOff(duration);

            onDisable?.Invoke();
        }

        protected void TurnOn(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration_Enable;

            _baseSettings.isOpen = true;

#if DoTweenInstalled
            _baseSettings.canvasGroup.DOKill();
            _baseSettings.canvasGroup.FadeUp(duration.Value);
#endif

            TurnOnUIAnimationElements_Async();

            if (_baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);
        }

        protected void TurnOff(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration_Disable;

            _baseSettings.isOpen = false;

            TurnOffUIAnimationElements();

#if DoTweenInstalled
            _baseSettings.canvasGroup.DOKill();

            if (duration.Value == 0)
            {
                _baseSettings.canvasGroup.FadeDownQuick(setActiveToFalse: _baseSettings.enableDisable);
            }
            else
            {
                _baseSettings.canvasGroup.FadeDown(duration.Value, setActiveToFalse: _baseSettings.enableDisable);
            }
#endif
        }

        protected async void TurnOnUIAnimationElements_Async()
        {
            foreach (var uiAnimationElement in _baseSettings.uIAnimationElements)
            {
                if (destroyCancellationToken.IsCancellationRequested == true) break;

                try
                {
                    uiAnimationElement?.TurnOn();

                    await AsyncHelper.DelayFloat(_baseSettings.uiAnimationElementForeachDelay);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Error animating ui animation element: " + ex, gameObject);
                }
            }
        }

        protected void TurnOffUIAnimationElements()
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

        protected virtual void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            onSubscribeToEvents?.Invoke();
        }

        protected virtual void UnsubscribeFromEvents()
        {
            onUnsubscribeFromEvents?.Invoke();
        }

        [Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration_Enable = 1;
            public float animationDuration_Disable = 0.25f;
            public float uiAnimationElementForeachDelay = 0.025f;

            [Header("Components")]
            [Required]
            public CanvasGroup canvasGroup;

            [Header("Debug")]
            public UIAnimationElement_Base[] uIAnimationElements;
            public bool isOpen = true;
        }
    }
}