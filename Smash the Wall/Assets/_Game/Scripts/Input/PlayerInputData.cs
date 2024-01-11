using Services;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInputData : InputsService
    {
        public InputActionsHolder input { get; private set; }

        public PlayerInputData()
        {
            input = new InputActionsHolder();
            Enable();
        }

        public void Enable()
        {
            input?.Enable();
        }

        public void Disable()
        {
            input?.Disable();
        }
    }
}