using Helpers;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Helpers
{
    public sealed class RandowmObjectPositionOnAwake : MonoBehaviour
    {
        [SerializeField] private List<Transform> _objects;

        [SerializeField] private Vector3 _minValues = new Vector3();
        [SerializeField] private Vector3 _maxValues = new Vector3();

        private async void Awake()
        {
            foreach (Transform obj in _objects)
            {
                await AsyncHelper.Delay();
                RandomizePosition(obj);
            }
        }

        public void RandomizePosition(Transform obj)
        {
            float x = Random.Range(_minValues.x, _maxValues.x);
            float y = Random.Range(_minValues.y, _maxValues.y);
            float z = Random.Range(_minValues.z, _maxValues.z);

            obj.transform.localPosition = new Vector3(x, y, z);
        }
    }
}