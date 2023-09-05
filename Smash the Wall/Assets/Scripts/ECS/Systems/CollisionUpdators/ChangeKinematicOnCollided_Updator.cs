using ECS.ComponentData.Picture.Piece;
using ECS.ComponentData.Projectile;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems.CollisionUpdators
{
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

        public void OnCreate(ref SystemState systemState)
        {
            instance = this;

            Disable();
        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            foreach (var (projectileComponent, projectileTransform) in SystemAPI.Query<RefRW<Projectile_ComponentData>, RefRW<LocalTransform>>())
            {
                foreach (var (picturePieceComponent, picturePiecesTransform, physicsMassOverride, rigidBodyAspect) in SystemAPI.Query
                    <
                        RefRW<PicturePiece_ComponentData>,
                        RefRW<LocalTransform>,
                        RefRW<PhysicsMassOverride>,
                        RigidBodyAspect
                    >())
                {
                    if (Vector3.Distance(projectileTransform.ValueRW.Position, picturePiecesTransform.ValueRW.Position) < 1.25f)
                    {
                        picturePieceComponent.ValueRW.isHit = true;
                        physicsMassOverride.ValueRW.SetVelocityToZero = 0;
                        rigidBodyAspect.LinearVelocity = new Unity.Mathematics.float3(GetRandomVelocityAxis(), GetRandomVelocityAxis(), GetRandomVelocityAxis());
                    }
                }
            }

            float GetRandomVelocityAxis()
            {
                float min = -10f;
                float max = 10f;

                return Random.Range(min, max);
            }
        }
    }
}