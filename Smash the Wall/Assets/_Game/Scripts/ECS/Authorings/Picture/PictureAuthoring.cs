using ECS.ComponentData.Picture;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using static DataClasses.Enums;

namespace ECS.Authoring
{
    public class PictureAuthoring : MonoBehaviour
    {
        [ShowInInspector] public PictureMode pictureMode = PictureMode.DestroyBorder;
        [ShowInInspector] public Color borderColor = Color.cyan;

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

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}