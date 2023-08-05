using DI;
using InGameStrings;
using PlayerInputHolder;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class ProjectileWeaponBase : WeaponBase
    {
        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] protected Input_SO _input;

        [Header("Components")]
        [SerializeField] protected Transform _gunPoint;

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