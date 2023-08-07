using DG.Tweening;
using DI;
using ECS.Systems.Spawners;
using InGameStrings;
using PlayerInputHolder;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace ProjectSripts
{
    public sealed class SlingShot : ProjectileWeaponBase, IDIDependent
    {
        [Header("Componenets")]
        [SerializeField] private Transform _elasticPart;

        [Header("Settings")]
        [SerializeField] private float _normalElasticZScalse;
        [SerializeField] private float _shootElasticZScalse;
        [SerializeField] private float _resetScaleDuration = 0.25f;
        [SerializeField] private float _shootScaleDuration = 0.1f;

        [Header("Debug")]
        [SerializeField] private bool _debug;

        private void OnEnable()
        {
            canShoot = false;
        }

        private void OnDisable()
        {
            DisableInput();
            canShoot = false;
        }

        public override void Initialize()
        {
            (this as IDIDependent).LoadDependencies();
        }

        public override void EnableInput()
        {
            base.EnableInput();

            _input.input.Player.Fire.performed += Fire;
            _input.input.Player.Fire.canceled += Fire;
            canShoot = false;

            ProjectileiGunBulletSpawner_System.instance.Initialize(this);
            ProjectileiGunBulletSpawner_System.instance.Enable();
        }

        public override void DisableInput()
        {
            base.DisableInput();

            _input.input.Player.Fire.performed -= Fire;
            _input.input.Player.Fire.canceled -= Fire;

            canShoot = false;

            ProjectileiGunBulletSpawner_System.instance.Disable();
            ProjectileiGunBulletSpawner_System.instance.Clear();
        }

        protected override void Fire(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() == true)
            {
                _elasticPart.DOScaleZ(_shootElasticZScalse, _shootScaleDuration);
            }
            else
            {
                if (_elasticPart.localScale.z == _shootElasticZScalse)
                {
                    canShoot = true;
                }

                _elasticPart.DOScaleZ(_normalElasticZScalse, _resetScaleDuration);
            }
        }

        public override void OnFired()
        {
            canShoot = false;
        }
    }
}