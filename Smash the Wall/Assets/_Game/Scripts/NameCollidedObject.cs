using Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public sealed class NameCollidedObject : MonoBehaviour
    {
        [SerializeField] private List<Collider> _collidedObjects = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            _collidedObjects.SafeAdd(other);
        }
    }
}