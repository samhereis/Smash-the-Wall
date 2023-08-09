using ECS.ComponentData.Picture;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class PictureAuthoring : MonoBehaviour
    {
        public class ShootBulletBaker : Baker<PictureAuthoring>
        {
            public override void Bake(PictureAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PictureComponentData
                {

                });
            }
        }

        [ContextMenu(nameof(Validate))]
        public void Validate()
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