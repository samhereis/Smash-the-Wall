using ECS.ComponentData.Picture;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static DataClasses.Enums;

namespace ECS.Authoring
{
    public class PictureAuthoring : MonoBehaviour
    {
        [SerializeField] public PictureMode pictureMode = PictureMode.DestroyBorder;
        [SerializeField] public Color borderColor = Color.cyan;

        [SerializeField] private List<MeshFilter> _meshesWithNoPositionVertexData = new List<MeshFilter>();

        public class ShootBulletBaker : Baker<PictureAuthoring>
        {
            public override void Bake(PictureAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PictureComponentData
                {

                });
            }
        }

        [Button]
        private void Validate()
        {
            foreach (var childTransformAuthoring in GetComponentsInChildren<PicturePieceAuthoring>(true))
            {
                childTransformAuthoring.Initialize();
            }

            foreach (var childTransformAuthoring in GetComponentsInChildren<ChildTransformAuthoring>(true))
            {
                childTransformAuthoring.Init();
            }
        }

        private void OnValidate()
        {
            ChechMeshes();
        }

        [Button]
        private void ChechMeshes()
        {
            _meshesWithNoPositionVertexData.Clear();

            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>(true))
            {
                if (HasPositionVertexData(meshFilter) == false)
                {
                    _meshesWithNoPositionVertexData.Add(meshFilter);
                }
            }
        }

        private bool HasPositionVertexData(MeshFilter meshFilter)
        {
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.sharedMesh;

                if (mesh != null)
                {
                    if (mesh.vertices == null || mesh.vertices.Length == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}