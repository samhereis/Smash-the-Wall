using Interfaces;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour, IUIWindow
    {
        public abstract void Enable(float? duration = null);
        public abstract void Disable(float? duration = null);
    }
}