using DG.Tweening;
using DI;
using ECS.Systems.Spawners;
using Helpers;
using Sound;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace ProjectSripts
{
    public sealed class Weapon_SingleBullet : ProjectileWeaponBase, IDIDependent
    {
        [Header("Componenets")]
        [SerializeField] private Transform _elasticPart;

        [Header("Settings")]
        [SerializeField] private float _normalElasticZScalse;
        [SerializeField] private float _shootElasticZScalse;
        [SerializeField] private float _resetScaleDuration = 0.25f;
        [SerializeField] private float _shootScaleDuration = 0.1f;

        [Header("Addressables")]
        [SerializeField] private AssetReferenceAudioClip _elasticStretchAudio;
        [SerializeField] private AssetReferenceAudioClip _elasticResetAudio;
        [SerializeField] private AssetReferenceAudioClip _shootAudio;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private SimpleSound _currentElasticStretchAudio;
        [SerializeField] private SimpleSound _currentElasticResetAudio;
        [SerializeField] private SimpleSound _currentShootAudio;

        private void OnEnable()
        {
            canShoot = false;
        }

        private void OnDisable()
        {
            DisableInput();
            canShoot = false;
        }

        public override async void Initialize()
        {
            base.Initialize();

            _currentElasticStretchAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_elasticStretchAudio));
            _currentElasticResetAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_elasticResetAudio));
            _currentShootAudio.SetAudioClip(await AddressablesHelper.GetAssetAsync<AudioClip>(_shootAudio));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _elasticPart = GetComponentsInChildren<Transform>().ToList().Find(x => x.gameObject.name == "elastic");
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
            if (UIHelper.IsPointOverUI(Touchscreen.current.position.ReadValue())) { return; }

            if (_elasticPart == null)
            {
                _elasticPart = GetComponentsInChildren<Transform>().ToList().Find(x => x.gameObject.name == "elastic");

                canShoot = true;
                return;
            }

            if (context.ReadValueAsButton() == true)
            {
                _elasticPart.DOScaleZ(_shootElasticZScalse, _shootScaleDuration);
                _trajectoryDisplayer.Enable(shootPosition);
                _soundPlayer.TryPlay(_currentElasticStretchAudio);
                VibrationHelper.LightVibration();
            }
            else
            {
                if (_elasticPart.localScale.z == _shootElasticZScalse) { canShoot = true; }

                _elasticPart.DOScaleZ(_normalElasticZScalse, _resetScaleDuration);
                _trajectoryDisplayer.Disable();
                _soundPlayer.TryPlay(_currentElasticResetAudio);
            }
        }

        public override void OnFired()
        {
            canShoot = false;
            _soundPlayer.TryPlay(_currentShootAudio);

            VibrationHelper.MeduimVibration();
        }
    }
}