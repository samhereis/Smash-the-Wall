using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected abstract void Fire(InputAction.CallbackContext context);

    }
}