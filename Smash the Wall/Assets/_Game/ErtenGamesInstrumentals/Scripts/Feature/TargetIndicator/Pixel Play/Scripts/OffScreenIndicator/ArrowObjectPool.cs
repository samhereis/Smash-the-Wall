using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace TargetIndicator
{
    public class ArrowObjectPool : MonoBehaviour
    {
        [SerializeField] public static ArrowObjectPool current;

        [SerializeField] public Indicator pooledObject;

        [FoldoutGroup("Settings"), SerializeField] private int _pooledAmount = 1;
        [FoldoutGroup("Settings"), SerializeField] private bool _willGrow = true;

        [FoldoutGroup("Debug"), SerializeField] private List<Indicator> _pooledObjects;

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            _pooledObjects = new List<Indicator>();

            for (int i = 0; i < _pooledAmount; i++)
            {
                Indicator arrow = Instantiate(pooledObject);
                arrow.transform.SetParent(transform, false);
                arrow.Activate(false);
                _pooledObjects.Add(arrow);
            }
        }

        public Indicator GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].Active)
                {
                    return _pooledObjects[i];
                }
            }

            if (_willGrow)
            {
                Indicator arrow = Instantiate(pooledObject);
                arrow.transform.SetParent(transform, false);
                arrow.Activate(false);
                _pooledObjects.Add(arrow);
                return arrow;
            }
            return null;
        }

        public void DeactivateAllPooledObjects()
        {
            foreach (Indicator arrow in _pooledObjects)
            {
                arrow.Activate(false);
            }
        }
    }
}
