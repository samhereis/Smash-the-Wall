using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class MusicVisualizer : MonoBehaviour
    {
        private Action<Transform, int> _do;

        private enum Axis { X, Y, Z }

        [Required]
        [SerializeField] private SpectrumData _spectrumData;

        [Required]
        [SerializeField] private Transform _prefab;

        [Required]
        [SerializeField] private Transform _parent;

        [FoldoutGroup("Settings"), ShowInInspector] private Axis _axis;
        [FoldoutGroup("Settings"), ShowInInspector] private float _multiplier = 1f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _indexMultiplier = 1f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _spawnCount = 1f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _smoothness = 0.03f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _minValue = 1;
        [FoldoutGroup("Settings"), ShowInInspector] private bool _useDefaultMultiplier;

        private List<Transform> _spawnedObjects = new List<Transform>();

        private void Awake()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                _spawnedObjects.Add(Instantiate(_prefab, _parent));
            }
        }

        private void OnEnable()
        {
            if (_axis == Axis.X)
            {
                _do = ScaleX;
            }
            else if (_axis == Axis.Y)
            {
                _do = ScaleY;
            }
            else
            {
                _do = ScaleZ;
            }
        }

        private void Update()
        {
            int index = 1;

            foreach (var spawnedObject in _spawnedObjects)
            {
                _do(spawnedObject, index);
                index++;
            }
        }

        private void ScaleX(Transform scalable, int index)
        {
            scalable.localScale = Vector3.Lerp(scalable.localScale, new Vector3(GetValue(index), scalable.localScale.y, scalable.localScale.z), _smoothness);
        }

        private void ScaleY(Transform scalable, int index)
        {
            scalable.localScale = Vector3.Lerp(scalable.localScale, new Vector3(scalable.localScale.x, GetValue(index), scalable.localScale.z), _smoothness);
        }

        private void ScaleZ(Transform scalable, int index)
        {
            scalable.localScale = Vector3.Lerp(scalable.localScale, new Vector3(scalable.localScale.x, scalable.localScale.y, GetValue(index)), _smoothness);
        }

        private float GetValue(int index)
        {
            return _minValue + (_spectrumData.frequencies[index] * _multiplier) * index * _indexMultiplier;
        }
    }
}