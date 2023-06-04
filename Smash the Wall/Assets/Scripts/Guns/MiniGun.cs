using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class MiniGun : WeaponBase
    {
        public static Transform shootPosition;
        public static bool canShoot;

        [SerializeField] private Transform _shootPosition;
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private int _rotationSpeed = 1;

        private InputSettings inputs;

        public float WaitBeforeFire = 2;

        private void Awake()
        {
            inputs = new InputSettings();
            shootPosition = _shootPosition;
        }

        private void OnEnable()
        {
            inputs.Player.Fire.performed += Fire;
            inputs.Player.Fire.canceled += Fire;

            inputs.Enable();

            canShoot = false;
        }

        private void OnDisable()
        {
            inputs.Player.Fire.performed -= Fire;
            inputs.Player.Fire.canceled -= Fire;

            inputs.Disable();

            canShoot = false;
        }

        private void Update()
        {
            if (_rotationSpeed > 1)
            {
                _gunPoint.Rotate(0, 0, -_rotationSpeed, Space.Self);
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
                _rotationSpeed = 0;

                canShoot = false;
            }
        }

        private IEnumerator FireCouroutine()
        {
            _rotationSpeed = 3;
            yield return new WaitForSecondsRealtime(WaitBeforeFire);
            canShoot = true;
        }
    }
}