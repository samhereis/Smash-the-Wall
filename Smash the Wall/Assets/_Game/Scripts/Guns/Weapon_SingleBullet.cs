using DependencyInjection;
using DG.Tweening;
using ECS.Systems.Spawners;
using Helpers;
using Sirenix.OdinInspector;
using SO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace ProjectSripts
{
    public sealed class Weapon_SingleBullet : ProjectileWeaponBase, ISelfValidator
    {
        [Header("Componenets")]
        [Required]
        [SerializeField] private Transform _elasticPart;

        [Header("Settings")]
        [SerializeField] private float _normalElasticZScalse;
        [SerializeField] private float _shootElasticZScalse;
        [SerializeField] private float _resetScaleDuration = 0.25f;
        [SerializeField] private float _shootScaleDuration = 0.1f;

        [Header("Sounds")]
        [SerializeField] private bool _debug;
        [SerializeField, Required] private Sound_String_SO _elasticStretchAudio;
        [SerializeField, Required] private Sound_String_SO _elasticResetAudio;
        [SerializeField, Required] private Sound_String_SO _shootAudio;

        [Inject] private VibrationHelper _vibrationHelper;

        public void Validate(SelfValidationResult result)
        {
            if (_elasticPart == null) { _elasticPart = GetComponentsInChildren<Transform>().ToList().Find(x => x.gameObject.name == "elastic"); }
        }

        private void OnEnable()
        {
            canShoot = false;
        }

        private void OnDisable()
        {
            DisableInput();
            canShoot = false;
        }

        public override void EnableInput()
        {
            base.EnableInput();

#if InputSystemInstalled
            _input.input.Player.Fire.started += Fire;
            _input.input.Player.Fire.canceled += Fire;
#endif

            canShoot = false;

            ProjectileiGunBulletSpawner_System.instance.Initialize(this);
            ProjectileiGunBulletSpawner_System.instance.Enable();
        }

        public override void DisableInput()
        {
            base.DisableInput();

#if InputSystemInstalled
            _input.input.Player.Fire.started -= Fire;
            _input.input.Player.Fire.canceled -= Fire;
#endif

            canShoot = false;

            ProjectileiGunBulletSpawner_System.instance.Disable();
            ProjectileiGunBulletSpawner_System.instance.Dispose();
        }

        protected override void Fire(InputAction.CallbackContext context)
        {
            if (UIHelper.IsPointOverUI() == true) { return; }

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
                _soundPlayer?.TryPlay(_elasticStretchAudio);
                _vibrationHelper?.LightVibration();
            }
            else
            {
                if (_elasticPart.localScale.z == _shootElasticZScalse) { canShoot = true; }

                _elasticPart.DOScaleZ(_normalElasticZScalse, _resetScaleDuration);
                _trajectoryDisplayer.Disable();
                _soundPlayer?.TryPlay(_elasticResetAudio);
            }
        }

        public override void OnFired()
        {
            canShoot = false;
            _soundPlayer?.TryPlay(_shootAudio);

            _vibrationHelper?.MeduimVibration();
        }
    }
}