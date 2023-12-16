using DG.Tweening;
using Helpers;
using Interfaces;
using System;
using UI.Canvases;
using UnityEngine;

namespace UI.Page
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowPageBase : MonoBehaviour, IUIWindow
    {
        [SerializeField] protected WindowPageBaseSettings _baseSettings = new WindowPageBaseSettings();

        public virtual void Enable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;

            _baseSettings.canvasGroup.DOKill();
            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            _baseSettings.canvasGroup.FadeUp(duration.Value);
        }

        public virtual void Disable(float? duration = null)
        {
            if (duration == null) duration = _baseSettings.animationDuration;

            if (duration.Value == 0)
            {
                _baseSettings.canvasGroup.FadeDownQuick();
                if (_baseSettings.enableDisable) gameObject.SetActive(false);
            }
            else
            {
                _baseSettings.canvasGroup.FadeDown(duration.Value, completeCallback: () =>
                {
                    if (_baseSettings.enableDisable) gameObject.SetActive(false);
                });
            }
        }

        [ContextMenu(nameof(Setup))]
        public void Setup()
        {
            if (_baseSettings.canvasGroup == null) _baseSettings.canvasGroup = GetComponent<CanvasGroup>();
            if (_baseSettings.parent == null) _baseSettings.parent = GetComponentInParent<CanvasWindowBase>(true);
        }

        [Serializable]
        protected class WindowPageBaseSettings
        {
            public CanvasWindowBase parent;
            public CanvasGroup canvasGroup;
            public float animationDuration = 0.5f;
            public bool enableDisable = true;
        }
    }
}
