using DependencyInjection;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class ProjectileWeaponBase : WeaponBase, INeedDependencyInjection
    {
        public override void Initialize()
        {
            DependencyContext.InjectDependencies(this);

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