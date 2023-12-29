using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Music
{
    public class MusicReactor_Scale : MonoBehaviour
    {
        private Action _do;

        private enum Axis { X, Y, Z }

        [ShowInInspector] private AFrequancyData _aFrequancyData;

        [FoldoutGroup("Settings")]
        [ShowInInspector] private float _smoothness = 0.25f;
        [ShowInInspector] private float _minValue = 1;
        [ShowInInspector] private float _multiplier = 1;
        [ShowInInspector] private bool _useDefaultMultiplier;
        [ShowInInspector] private Axis _axis;

        private float _value => _minValue + (_aFrequancyData.value * _multiplier);

        private void Awake()
        {
            if (_useDefaultMultiplier) _multiplier = _aFrequancyData.defaultMultiplier;
        }

        private void OnEnable()
        {
            if (_axis == Axis.X)
            {
                _do = () => { transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_value, transform.localScale.y, transform.localScale.z), _smoothness); };
            }
            else if (_axis == Axis.Y)
            {
                _do = () => { transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, _value, transform.localScale.z), _smoothness); };
            }
            else
            {
                _do = () => { transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, _value), _smoothness); };
            }
        }

        private void Update()
        {
            _do();
        }
    }
}