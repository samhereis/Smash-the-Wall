using DG.Tweening;
using Helpers;
using Interfaces;
using System;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour, IUIWindow
    {
        protected static Action<PopupBase> _onAPopupOpen;

        [SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

        protected virtual void Awake()
        {
            _onAPopupOpen += OnAPopupOpen;
        }

        protected virtual void OnDestroy()
        {
            _onAPopupOpen -= OnAPopupOpen;
        }

        public virtual void OnAPopupOpen(PopupBase popup)
        {
            if (popup != this) Close();
        }

        public void Open()
        {
            Enable();
        }

        public void Close()
        {
            Disable();
        }

        public virtual void Enable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;
            if (_baseSettings.notifyOthers == true) _onAPopupOpen?.Invoke(this);

            _baseSettings.background?.DOKill();
            _baseSettings.holder?.DOKill();

            if (_baseSettings.enableDisable) gameObject.SetActive(true);

            _baseSettings.background?.FadeUp(duration.Value);
            _baseSettings.holder?.DOScale(1, duration.Value);
        }

        public virtual void Disable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;

            if (duration.Value == 0)
            {
                _baseSettings.background?.FadeDownQuick();
                if (_baseSettings.holder != null) _baseSettings.holder.localScale = Vector3.zero;

                if (_baseSettings.enableDisable) gameObject.SetActive(false);
            }
            else
            {
                _baseSettings.background?.DOKill();
                _baseSettings.holder?.DOKill();

                _baseSettings.background?.FadeDown(duration.Value);
                _baseSettings.holder?.DOScale(0, duration.Value)?.OnComplete(() =>
                {
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                });
            }

        }

        [System.Serializable]
        protected class BaseSettings
        {
            public CanvasGroup background;
            public Transform holder;

            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration = 0.5f;
        }
    }
}