using DI;
using Helpers;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class ProjectileWeaponBase : WeaponBase
    {
        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();

            base.Initialize();
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