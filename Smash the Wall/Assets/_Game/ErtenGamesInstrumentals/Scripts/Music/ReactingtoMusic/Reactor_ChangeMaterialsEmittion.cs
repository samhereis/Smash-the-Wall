using Sirenix.OdinInspector;
using UnityEngine;

namespace Music.Reactors
{
    public class Reactor_ChangeMaterialsEmittion : MonoBehaviour
    {
        [FoldoutGroup("Material Settings"), ShowInInspector] private Material _material;
        [FoldoutGroup("Material Settings"), ShowInInspector] private string _property;

        [FoldoutGroup("SO"), ShowInInspector] private AFrequancyData _aFrequancyData;

        [FoldoutGroup("Settings"), ShowInInspector] private float _minValue = 1.5f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _multiplier;
        [FoldoutGroup("Settings"), ShowInInspector] private Color _originalColor;

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