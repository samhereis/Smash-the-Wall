using DI;
using ECS.Systems.Spawners;
using InGameStrings;
using PlayerInputHolder;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class MiniGun : WeaponBase, IDIDependent
    {
        public static Transform shootPosition;
        public static bool canShoot;

        [Header("DI")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _input;

        [Header("Components")]
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private Transform _gunPoint;

        [Header("Settings")]
        [SerializeField] private float _rotationSpeed = 1;
        [SerializeField] private float _waitBeforeFire = 2;

        private float _currentRotationSpeed;

        private void Awake()
        {
            shootPosition = _shootPosition;
        }

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private void OnEnable()
        {
            _input.input.Player.Fire.performed += Fire;
            _input.input.Player.Fire.canceled += Fire;
            canShoot = false;

            if (MiniGunBulletSpawner_System.instance.isActive == false) { MiniGunBulletSpawner_System.instance.Enable(); }
        }

        private void OnDisable()
        {
            _input.input.Player.Fire.performed -= Fire;
            _input.input.Player.Fire.canceled -= Fire;

            canShoot = false;

            if (MiniGunBulletSpawner_System.instance.isActive == false) { MiniGunBulletSpawner_System.instance.Disable(); }
        }

        private void Update()
        {
            if (_currentRotationSpeed > 1)
            {
                _gunPoint.Rotate(0, 0, -_currentRotationSpeed, Space.Self);
            }
        }

        protected override void Fire(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() == true)
            {
                StartCoroutine(FireCouroutine());
            }
            else
            {
                StopAllCoroutines();
                _currentRotationSpeed = 0;

                canShoot = false;
            }
        }

        private IEnumerator FireCouroutine()
        {
            _currentRotationSpeed = _rotationSpeed;
            yield return new WaitForSecondsRealtime(_waitBeforeFire);
            canShoot = true;
        }
    }
}