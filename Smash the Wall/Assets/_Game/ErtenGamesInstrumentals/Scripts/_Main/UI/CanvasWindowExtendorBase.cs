using System;
using UnityEngine;

namespace UI.Canvases
{
    public abstract class CanvasWindowExtendorBase<TCanvasWindow> : MonoBehaviour where TCanvasWindow : CanvasWindowBase
    {
        public TCanvasWindow window
        {
            get
            {
                if (_baseSettings.window == null) { _baseSettings.window = GetComponent<TCanvasWindow>(); }

                return _baseSettings.window;
            }
        }

        [SerializeField] protected CanvasWindowExtendorBaseSettings _baseSettings = new CanvasWindowExtendorBaseSettings();

        [Serializable]
        public class CanvasWindowExtendorBaseSettings
        {
            public TCanvasWindow window;
        }
    }
}