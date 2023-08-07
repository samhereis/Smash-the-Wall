using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IInitializable, IHasInput
    {
        [field: SerializeField, Header("Current State")] public bool canShoot { get; protected set; }
        [field: SerializeField] public Transform shootPosition { get; protected set; }

        public abstract void Initialize();
        public abstract void EnableInput();
        public abstract void DisableInput();
        protected abstract void Fire(InputAction.CallbackContext context);
        public abstract void OnFired();

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}