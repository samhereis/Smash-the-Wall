using DI;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class ProjectileWeaponBase : WeaponBase
    {
        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public override void EnableInput()
        {

        }

        public override void DisableInput()
        {

        }

        protected override void Fire(InputAction.CallbackContext context)
        {

        }

        public override void OnFired()
        {

        }
    }
}