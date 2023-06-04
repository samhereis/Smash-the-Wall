using UnityEngine;

namespace Music.Reactors
{
    public class Reactor_ChangeMaterialsEmittion : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private string _property;

        [Header("SO")]
        [SerializeField] private AFrequancyData _aFrequancyData;

        [Header("Settings")]
        [SerializeField] private float _minValue = 1.5f;
        [SerializeField] private float _multiplier;
        [SerializeField] private Color _originalColor;

        private void OnValidate()
        {
            _originalColor = _material.color;
        }

        private void Update()
        {
            _material.SetColor(_property, new Color(_originalColor.r, _originalColor.g, _originalColor.b) * (_minValue + (_aFrequancyData.value * _multiplier)));
        }
    }
}