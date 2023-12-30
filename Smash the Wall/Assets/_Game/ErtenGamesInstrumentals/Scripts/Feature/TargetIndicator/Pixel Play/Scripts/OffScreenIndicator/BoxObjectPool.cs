using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace TargetIndicator
{
    public class BoxObjectPool : MonoBehaviour
    {
        // TODO: Make base class for Arrow and Box object pools

        public static BoxObjectPool current;

        [FoldoutGroup("Settings"), SerializeField] private Indicator _pooledObject;
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
                Indicator box = Instantiate(_pooledObject);
                box.transform.SetParent(transform, false);
                box.Activate(false);
                _pooledObjects.Add(box);
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
                Indicator box = Instantiate(_pooledObject);
                box.transform.SetParent(transform, false);
                box.Activate(false);
                _pooledObjects.Add(box);
                return box;
            }

            return null;
        }

        public void DeactivateAllPooledObjects()
        {
            foreach (Indicator box in _pooledObjects)
            {
                box.Activate(false);
            }
        }
    }
}