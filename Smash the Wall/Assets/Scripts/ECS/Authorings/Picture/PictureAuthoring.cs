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

        private void OnValidate()
        {
            Validate();
        }

        [ContextMenu(nameof(Validate))]
        public void Validate()
        {
            foreach (var picturePieceAuthoring in GetComponentsInChildren<PicturePieceAuthoring>(true))
            {
                picturePieceAuthoring.Init();
            }

            foreach (var childTransformAuthoring in GetComponentsInChildren<ChildTransformAuthoring>(true))
            {
                childTransformAuthoring.Init();
            }
        }
    }
}