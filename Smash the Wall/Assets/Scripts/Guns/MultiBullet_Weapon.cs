using DI;
using ECS.Systems.Spawners;
using Helpers;
using Interfaces;
using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class MultiBullet_Weapon : ProjectileWeaponBase, IDIDependent, IInitializable
    {
        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 1;
        [SerializeField] private float _waitBeforeFire = 2;
        [SerializeField] private float _fireRate = 0.25f;

        [Header("Addressables")]
        [SerializeField] private AssetReferenceAudioClip _preShootAudio;
        [SerializeField] private AssetReferenceAudioClip _resetAudio;
        [SerializeField] private AssetReferenceAudioClip _shootAudio;

        [Header("Debug")]
        [SerializeField] private SimpleSound _currentPreShootAudio;
        [SerializeField] private SimpleSound _currentResetAudio;
        [SerializeField] private SimpleSound _currentShootAudio;

        private float _currentRotationSpeed;

        private async void OnEnable()
        {
            canShoot = false;

            _currentResetAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_resetAudio));
            _currentShootAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_shootAudio));
            _currentPreShootAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_preShootAudio));
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
            if (UIHelper.IsPointOverUI(Touchscreen.current.position.ReadValue())) { return; }

            if (context.ReadValueAsButton() == true)
            {
                StartCoroutine(FireCouroutine());

                _trajectoryDisplayer.Enable(shootPosition);

                _soundPlayer.TryPlay(_currentPreShootAudio);
                VibrationHelper.LightVibration();
            }
            else
            {
                StopAllCoroutines();
                _currentRotationSpeed = 0;

                canShoot = false;

                _trajectoryDisplayer.Disable();

                _soundPlayer.TryPlay(_currentResetAudio);
                VibrationHelper.LightVibration();
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

            _soundPlayer.TryPlay(_currentShootAudio);
        }
    }
}