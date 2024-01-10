using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Services
{
    public class InputsService
    {
        public Action onUIBackPressed;

        public readonly UnityEvent<bool> onInputStatusChanged = new UnityEvent<bool>();

#if InputSystemInstalled
        public InputActionsHolder input { get; private set; }
#else
        public dynamic input { get; private set; }
#endif

        public InputsService()
        {
            input = new InputActionsHolder();

            Enable();
        }

        public void Enable()
        {
            input?.Enable();
            onInputStatusChanged?.Invoke(true);
        }

        public void Disable()
        {
            input?.Disable();
            onInputStatusChanged?.Invoke(false);
        }
    }
}