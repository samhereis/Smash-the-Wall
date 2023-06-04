using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Samhereis.Helpers;
using DG.Tweening;
using UI.Window;
using UI.Canvases;
using Helpers;

namespace UI.Page
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowPageBase : MonoBehaviour
    {
        [SerializeField] protected WindowPageBaseSettings _baseSettings = new WindowPageBaseSettings();

        public virtual void Open(float? duration = null)
        {
            if (duration == null) duration = _baseSettings._animationDuration;

            _baseSettings._canvasGroup.DOKill();
            if (_baseSettings.enableDisable) gameObject.SetActive(true);
            _baseSettings._canvasGroup.FadeUp(duration.Value);
        }

        public virtual void Close(float? duration = null)
        {
            if (duration == null) duration = _baseSettings._animationDuration;

            _baseSettings._canvasGroup.FadeDown(duration.Value, completeCallback: () =>
            {
                if (_baseSettings.enableDisable) gameObject.SetActive(false);
            });
        }

        [ContextMenu(nameof(Setup))]
        public void Setup()
        {
            if (_baseSettings._canvasGroup == null) _baseSettings._canvasGroup = GetComponent<CanvasGroup>();
            if (_baseSettings._parent == null) _baseSettings._parent = GetComponentInParent<CanvasWindowBase>(true);
        }

        [Serializable]
        protected class WindowPageBaseSettings
        {
            public CanvasWindowBase _parent;
            public CanvasGroup _canvasGroup;
            public float _animationDuration = 0.5f;
            public bool enableDisable = true;
        }
    }
}
