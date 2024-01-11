using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Music
{
    public class MusicReactor_Scale : MonoBehaviour
    {
        private Action _do;

        private enum Axis { X, Y, Z }

        [SerializeField] private AFrequancyData _aFrequancyData;

        [FoldoutGroup("Settings")]
        [SerializeField] private float _smoothness = 0.25f;
        [SerializeField] private float _minValue = 1;
        [SerializeField] private float _multiplier = 1;
        [SerializeField] private bool _useDefaultMultiplier;
        [SerializeField] private Axis _axis;

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