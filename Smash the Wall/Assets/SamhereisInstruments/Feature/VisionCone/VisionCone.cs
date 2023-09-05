using UnityEngine;

namespace Feature.VisionCone
{
    public class VisionCone : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Material _visionConeMaterial;

        [Header("Settings")]
        [SerializeField] private float _visionRange;
        [SerializeField] private float _visionAngle;
        [SerializeField] private int _visionConeResolution = 120;

        [Space(10)]
        [SerializeField] private LayerMask _visionObstructingLayer;

        Mesh _visionConeMesh;
        MeshFilter _meshFilter;

        private void Start()
        {
            gameObject.AddComponent<MeshRenderer>().material = _visionConeMaterial;
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _visionConeMesh = new Mesh();
            _visionAngle *= Mathf.Deg2Rad;
        }

        private void Update()
        {
            DrawVisionCone();
        }

        private void DrawVisionCone()
        {
            int[] triangles = new int[(_visionConeResolution - 1) * 3];
            Vector3[] Vertices = new Vector3[_visionConeResolution + 1];
            Vertices[0] = Vector3.zero;
            float Currentangle = -_visionAngle / 2;
            float angleIcrement = _visionAngle / (_visionConeResolution - 1);
            float Sine;
            float Cosine;

            for (int i = 0; i < _visionConeResolution; i++)
            {
                Sine = Mathf.Sin(Currentangle);
                Cosine = Mathf.Cos(Currentangle);
                Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
                Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);

                if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, _visionRange, _visionObstructingLayer))
                {
                    Vertices[i + 1] = VertForward * hit.distance;
                }
                else
                {
                    Vertices[i + 1] = VertForward * _visionRange;
                }

                Currentangle += angleIcrement;
            }

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }

            _visionConeMesh.Clear();
            _visionConeMesh.vertices = Vertices;
            _visionConeMesh.triangles = triangles;
            _meshFilter.mesh = _visionConeMesh;
        }
    }
}