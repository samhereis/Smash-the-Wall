using UnityEngine;

namespace Gameplay
{
    public class ObjectRotator : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private Vector3 _rotationValue;
        [SerializeField] private Transform _mesh;

        private void Update()
        {
            _mesh.eulerAngles += _rotationValue * Time.deltaTime;
        }
    }
}