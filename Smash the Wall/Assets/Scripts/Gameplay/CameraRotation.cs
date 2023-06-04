using Backend;
using DI;
using PlayerInputHolder;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CameraRotation : MonoBehaviour, IDIDependent
    {
        [SerializeField] private Transform PlayerBody;
        [SerializeField][Range(0, 1)] private float _mouseSensitivity = 0.2f;

        [Header("DI")]
        [DI(InGameStrings.DIStrings.inputHolder)][SerializeField] private Input_SO inputs;

        private float _mouseX;
        private float _mouseY;
        private float _xRotation;
        private float _yRotation;

        private void Awake()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private void OnEnable()
        {
            inputs.input.Player.Look.performed += Look;
            inputs.input.Player.Look.canceled += Look;
        }

        private void OnDisable()
        {
            inputs.input.Player.Look.performed -= Look;
            inputs.input.Player.Look.canceled -= Look;
        }

        private void Look(InputAction.CallbackContext context)
        {
            var contextValue = context.ReadValue<Vector2>() * _mouseSensitivity;

            _mouseX = contextValue.x;
            _mouseY = contextValue.y;

            UpdateLook();
        }

        private void UpdateLook()
        {
            _xRotation += _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -30f, 30f);

            _yRotation += _mouseX;
            _yRotation = Mathf.Clamp(_yRotation, -30f, 30f);

            transform.localRotation = Quaternion.Euler(-_xRotation, 0f, 0f);
            PlayerBody.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
        }
    }
}