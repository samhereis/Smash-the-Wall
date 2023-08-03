using DG.Tweening;
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
            if (popup != this) Disable();
        }

        public virtual void Enable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;
            if (_baseSettings.notifyOthers == true) _onAPopupOpen?.Invoke(this);

            transform.DOKill();

            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            transform.DOScale(1, duration.Value);
        }

        public virtual void Disable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;

            if (duration.Value == 0)
            {
                transform.localScale = Vector3.zero;
                if (_baseSettings.enableDisable) gameObject.SetActive(false);
            }
            else
            {
                transform.DOKill();

                transform.DOScale(0, duration.Value)?.OnComplete(() =>
                {
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                });
            }

        }

        [System.Serializable]
        protected class BaseSettings
        {
            [Header("Settings")]
            public bool enableDisable = true;
            public bool notifyOthers = true;
            public float animationDuration = 0.5f;
        }
    }
}