using DG.Tweening;
using System;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        protected static Action<PopupBase> onAPopupOpen;

        [SerializeField] protected BaseSettings baseSettings = new BaseSettings();

        protected virtual void Awake()
        {
            onAPopupOpen += OnAPopupOpen;
        }

        protected virtual void OnDestroy()
        {
            onAPopupOpen -= OnAPopupOpen;
        }

        public virtual void OnAPopupOpen(PopupBase popup)
        {
            if (popup != this) Disable();
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
            if (duration == null) duration = baseSettings.animationDuration;
            if (baseSettings.notifyOthers == true) onAPopupOpen?.Invoke(this);

            transform.DOKill();

            if (baseSettings.enableDisable) gameObject.SetActive(true);
            transform.DOScale(1, duration.Value);
        }

        public virtual void Disable(float? duration = null)
        {
            if (duration == null) duration = baseSettings.animationDuration;

            if (duration.Value == 0) transform.localScale = Vector3.zero;

            transform.DOKill();

            transform.DOScale(0, duration.Value)?.OnComplete(() =>
            {
                if (baseSettings.enableDisable) gameObject.SetActive(false);
            }); ;
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