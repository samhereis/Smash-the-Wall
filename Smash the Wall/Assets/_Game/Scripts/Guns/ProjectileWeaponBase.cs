using DependencyInjection;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class ProjectileWeaponBase : WeaponBase, IDIDependent
    {
        public override void Initialize()
        {
            DependencyInjector.InjectDependencies(this);

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