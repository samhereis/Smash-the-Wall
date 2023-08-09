using ECS.ComponentData.Picture.Piece;
using ECS.ComponentData.Projectile;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems.CollisionUpdators
{
    [BurstCompile]
    public partial struct ChangeKinematicOnCollided_Updator : ISystem, IEnableableSystem
    {
        public static ChangeKinematicOnCollided_Updator instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public void Enable()
        {
            _isActive = true;
        }

        public void Disable()
        {
            _isActive = false;
        }

        [BurstCompile]
        public void OnCreate(ref SystemState systemState)
        {
            instance = this;

            Disable();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            foreach (var (projectileComponent, projectileTransform) in SystemAPI.Query<RefRW<Projectile_ComponentData>, RefRW<LocalTransform>>())
            {
                foreach (var (picturePieceComponent, picturePiecesTransform) in SystemAPI.Query<RefRW<PicturePiece_ComponentData>, RefRW<LocalTransform>>())
                {
                    if (picturePieceComponent.ValueRO.isHit == true) continue;

                    if (Vector3.Distance(projectileTransform.ValueRW.Position, picturePiecesTransform.ValueRW.Position) < 1)
                    {
                        picturePieceComponent.ValueRW.isHit = true;
                    }
                }
            }
        }
    }
}