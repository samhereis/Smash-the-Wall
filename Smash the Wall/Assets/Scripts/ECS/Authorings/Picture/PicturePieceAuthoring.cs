using ECS.ComponentData.Picture.Piece;
using Helpers;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

namespace ECS.Authoring
{
    [RequireComponent(typeof(ChildTransformAuthoring))]
    [RequireComponent(typeof(PhysicsBodyAuthoring))]
    [RequireComponent(typeof(PhysicsShapeAuthoring))]
    [DisallowMultipleComponent]
    public class PicturePieceAuthoring : MonoBehaviour
    {
        public enum PicturePieceType { WhatNeedsToBeDestroyed, WhatNeedsToStay }

        public class ShootBulletBaker : Baker<PicturePieceAuthoring>
        {
            public override void Bake(PicturePieceAuthoring authoring)
            {
                //authoring.Init();

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PicturePiece_ComponentData
                {
                    isHit = authoring.isHit,
                    isKinematic = authoring.isKinematic,
                });

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PhysicsMassOverride { IsKinematic = 0 });

                if (authoring.picturePieceType == PicturePieceType.WhatNeedsToBeDestroyed)
                {
                    AddComponent(GetEntity(TransformUsageFlags.Dynamic), new WhatNeedsToBeDestroyed_ComponentData());
                }
                else if (authoring.picturePieceType == PicturePieceType.WhatNeedsToStay)
                {
                    AddComponent(GetEntity(TransformUsageFlags.Dynamic), new WhatNeedsToStay_ComponentData());
                }
            }
        }

        [field: SerializeField] public PicturePieceType picturePieceType { get; private set; } = PicturePieceType.WhatNeedsToBeDestroyed;

        [field: SerializeField] public bool isKinematic { get; private set; } = true;
        [field: SerializeField] public bool isHit { get; private set; }

        [ContextMenu(nameof(Init))]
        public void Init()
        {
# if UNITY_EDITOR
            var physicsShape = GetComponent<PhysicsShapeAuthoring>();
            physicsShape.SetConvexHull(new ConvexHullGenerationParameters());
            physicsShape.CollisionResponse = CollisionResponsePolicy.CollideRaiseCollisionEvents;

            physicsShape.InitializeConvexHullGenerationParameters();

            this.TrySetDirty();
#endif
        }

    }
}