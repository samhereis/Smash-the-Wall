#if DoTweenInstalled
using DG.Tweening;
#endif

using DependencyInjection;
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
    public abstract class CanvasWindowBase : MonoBehaviour, IDIDependent, IUIWindow, IInitializable, ISelfValidator
    {
        public Action onEnable;
        public Action onDisable;

        public Action onSubscribeToEvents;
        public Action onUnsubscribeFromEvents;

        public static Action<CanvasWindowBase> onAWindowOpen { get; private set; }

        [FoldoutGroup("BaseSettings")][SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

#if UNITY_EDITOR

        [FoldoutGroup("BaseSettings")][SerializeField] private List<Button> _buttons = new List<Button>();
        [FoldoutGroup("BaseSettings")][ShowInInspector] protected ScaleByPercentage[] _scaleByPercentages;

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

        public virtual void Initialize()
        {
            DependencyInjector.InjectDependencies(this);
            _baseSettings.Initialize();
        }

        public virtual void OnAWindowOpen(IUIWindow uIWIndow)
        {
            if (uIWIndow != this as IUIWindow)
            {
                Disable();
            }
        }

        public virtual void Enable(float? duration = null)
        {
            TurnOn(duration);

            onEnable?.Invoke();
        }

        public virtual void Disable(float? duration = null)
        {
            TurnOff(duration);

            onDisable?.Invoke();
        }

        protected void TurnOn(float? duration = null)
        {
            if (_baseSettings.isOpen == true) return;
            if (duration == null) duration = _baseSettings.animationDuration_Enable;

#if DoTweenInstalled
            _baseSettings.canvasGroup.DOKill();
            _baseSettings.canvasGroup.FadeUp(duration.Value,

            setActiveToTrue: _baseSettings.enableDisable,
                completeCallback: () =>
            {
                _baseSettings.isOpen = true;
            });
#endif

            TurnOnUIAnimationElements_Async();

            if (_baseSettings.notifyOthers == true) onAWindowOpen?.Invoke(this);
        }

        protected void TurnOff(float? duration = null)
        {
            if (_baseSettings.isOpen == false) return;
            if (duration == null) duration = _baseSettings.animationDuration_Disable;

            TurnOffUIAnimationElements();

#if DoTweenInstalled
            _baseSettings.canvasGroup.DOKill();

            if (duration.Value == 0)
            {
                _baseSettings.canvasGroup.FadeDownQuick(setActiveToFalse: _baseSettings.enableDisable);
                OnClosed();
            }
            else
            {
                _baseSettings.canvasGroup.FadeDown(duration.Value,
                    setActiveToFalse: _baseSettings.enableDisable,
                    completeCallback: () =>
                {
                    OnClosed();
                });
            }
            
            void OnClosed()
            {
                _baseSettings.isOpen = false;
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
        protected class BaseSettings : IDIDependent, IInitializable
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration_Enable = 1;
            public float animationDuration_Disable = 0.25f;
            public float uiAnimationElementForeachDelay = 0.025f;

            [Header("Components")]
            public bool isOpen = true;

            [Header("Components")]

            [Required]
            public CanvasGroup canvasGroup;

            [Header("Debug")]
            public UIAnimationElement_Base[] uIAnimationElements;

            public void Initialize()
            {
                DependencyInjector.InjectDependencies(this);
            }
        }
    }
}