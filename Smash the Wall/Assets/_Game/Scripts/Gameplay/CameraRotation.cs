using Configs;
using DependencyInjection;
using Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CameraRotation : MonoBehaviour, INeedDependencyInjection
    {
        public float _gunRotationSpeed => _gameConfigs == null ? 0.2f : _gameConfigs.gameSettings.gunRotationSpeed.currentValue;

        [Required]
        [SerializeField] private Transform _xRotatedObject;

        [Required]
        [SerializeField] private Transform _yRotatedObject;

        [Header("DI")]
#if InputSystemInstalled
        [Inject]private InputsService _inputsHolder;
#endif
        [Inject]private GameConfigs _gameConfigs;

        private float _mouseX;
        private float _mouseY;
        private float _xRotation;
        private float _yRotation;

        private void OnEnable()
        {
            DependencyContext.InjectDependencies(this);

#if InputSystemInstalled
            _inputsHolder.input.Player.Look.performed += Look;
            _inputsHolder.input.Player.Look.canceled += Look;
#endif
        }

        private void OnDisable()
        {
#if InputSystemInstalled
            _inputsHolder.input.Player.Look.performed -= Look;
            _inputsHolder.input.Player.Look.canceled -= Look;
#endif
        }

        private void Look(InputAction.CallbackContext context)
        {
            var contextValue = context.ReadValue<Vector2>() * _gunRotationSpeed;

            _mouseX = contextValue.x;
            _mouseY = contextValue.y;

            UpdateLook();
            UpdateLook();
        }

        private void UpdateLook()
        {
            _xRotation += _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -30f, 30f);

            _yRotation += _mouseX;
            _yRotation = Mathf.Clamp(_yRotation, -30f, 30f);

            _xRotatedObject.localRotation = Quaternion.Euler(-_xRotation, 0f, 0f);
            _yRotatedObject.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
        }
    }
}