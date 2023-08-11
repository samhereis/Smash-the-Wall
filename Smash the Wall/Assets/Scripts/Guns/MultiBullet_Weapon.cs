using DI;
using ECS.Systems.Spawners;
using Helpers;
using Identifiers;
using Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class MultiBullet_Weapon : ProjectileWeaponBase, IDIDependent, IInitializable
    {
        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 1;
        [SerializeField] private float _waitBeforeFire = 2;
        [SerializeField] private float _fireRate = 0.25f;

        private float _currentRotationSpeed;

        private void OnEnable()
        {
            canShoot = false;
        }

        private void OnDisable()
        {
            canShoot = false;
            DisableInput();
        }

        private void Update()
        {
            if (_gunPoint == null) { return; }
            if (_currentRotationSpeed == 0) { return; }

            _gunPoint.Rotate(0, 0, -_currentRotationSpeed, Space.Self);
        }

        public override void EnableInput()
        {
            base.EnableInput();

            _input.input.Player.Fire.performed += Fire;
            _input.input.Player.Fire.canceled += Fire;

            ProjectileiGunBulletSpawner_System.instance.Initialize(this);
            ProjectileiGunBulletSpawner_System.instance.Enable();
        }

        public override void DisableInput()
        {
            base.DisableInput();

            _input.input.Player.Fire.performed -= Fire;
            _input.input.Player.Fire.canceled -= Fire;

            ProjectileiGunBulletSpawner_System.instance.Disable();
            ProjectileiGunBulletSpawner_System.instance.Clear();
        }

        protected override void Fire(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() == true)
            {
                StartCoroutine(FireCouroutine());

                _trajectoryDisplayer.Enable(shootPosition);
            }
            else
            {
                StopAllCoroutines();
                _currentRotationSpeed = 0;

                canShoot = false;

                _trajectoryDisplayer.Disable();
            }

            IEnumerator FireCouroutine()
            {
                _currentRotationSpeed = _rotationSpeed;
                yield return new WaitForSecondsRealtime(_waitBeforeFire);
                canShoot = true;
            }
        }

        public override void OnFired()
        {
            base.OnFired();

            canShoot = false;
            StartCoroutine(ResetCanFireCoroutine());

            IEnumerator ResetCanFireCoroutine()
            {
                yield return new WaitForSecondsRealtime(_fireRate);
                canShoot = true;
            }
        }
    }
}