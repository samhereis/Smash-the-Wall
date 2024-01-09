using DependencyInjection;
using ECS.Systems.Spawners;
using Helpers;
using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class MultiBullet_Weapon : ProjectileWeaponBase
    {
        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 1;
        [SerializeField] private float _waitBeforeFire = 2;
        [SerializeField] private float _fireRate = 0.25f;

        [Header("Sound")]
        [SerializeField] private SimpleSound _preShootAudio;
        [SerializeField] private SimpleSound _resetAudio;
        [SerializeField] private SimpleSound _shootAudio;

        [Inject] private VibrationHelper _vibrationHelper;

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

#if InputSystemInstalled
            _input.input.Player.Fire.performed += Fire;
            _input.input.Player.Fire.canceled += Fire;
#endif

            ProjectileiGunBulletSpawner_System.instance.Initialize(this);
            ProjectileiGunBulletSpawner_System.instance.Enable();
        }

        public override void DisableInput()
        {
            base.DisableInput();

#if InputSystemInstalled
            _input.input.Player.Fire.performed -= Fire;
            _input.input.Player.Fire.canceled -= Fire;
#endif

            ProjectileiGunBulletSpawner_System.instance.Disable();
            ProjectileiGunBulletSpawner_System.instance.Dispose();
        }

        protected override void Fire(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR == false

            if (UIHelper.IsPointOverUI(Touchscreen.current.position.ReadValue())) { return; }

#endif

            if (context.ReadValueAsButton() == true)
            {
                StartCoroutine(FireCouroutine());

                _trajectoryDisplayer.Enable(shootPosition);

                _soundPlayer.TryPlay(_preShootAudio);
                _vibrationHelper.LightVibration();
            }
            else
            {
                StopAllCoroutines();
                _currentRotationSpeed = 0;

                canShoot = false;

                _trajectoryDisplayer.Disable();

                _soundPlayer.TryPlay(_resetAudio);
                _vibrationHelper.LightVibration();
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

            _soundPlayer.TryPlay(_shootAudio);
        }
    }
}