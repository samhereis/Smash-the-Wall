using Sirenix.OdinInspector;
using UnityEngine;

namespace Music.Reactors
{
    public class Reactor_ChangeMaterialsEmittion : MonoBehaviour
    {
        [FoldoutGroup("Material Settings"), SerializeField] private Material _material;
        [FoldoutGroup("Material Settings"), SerializeField] private string _property;

        [FoldoutGroup("SO"), SerializeField] private AFrequancyData _aFrequancyData;

        [FoldoutGroup("Settings"), SerializeField] private float _minValue = 1.5f;
        [FoldoutGroup("Settings"), SerializeField] private float _multiplier;
        [FoldoutGroup("Settings"), SerializeField] private Color _originalColor;

        private void Awake()
        {
            _originalColor = _material.color;
        }

        private void Update()
        {
            _material.SetColor(_property,
                new Color(_originalColor.r, _originalColor.g, _originalColor.b) * (_minValue + (_aFrequancyData.value * _multiplier)));
        }
    }
}