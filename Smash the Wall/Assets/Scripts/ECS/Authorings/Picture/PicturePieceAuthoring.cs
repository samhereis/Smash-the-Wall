using ECS.ComponentData.Other;
using ECS.ComponentData.Picture.Piece;
using Helpers;
using Interfaces;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using MeshCollider = UnityEngine.MeshCollider;

namespace ECS.Authoring
{
    [RequireComponent(typeof(ChildTransformAuthoring))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class PicturePieceAuthoring : MonoBehaviour, IInitializable
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
                });

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PhysicsMassOverride { });

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

        [ContextMenu(nameof(Initialize))]
        public void Initialize()
        {
            GetComponent<MeshCollider>().convex = true;
            MonobehaviorHelper.DeleteAllMissingScripts(gameObject);
            this.TrySetDirty();
        }
    }
}