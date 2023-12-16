using ECS.ComponentData.Other;
using ECS.ComponentData.Picture.Piece;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems.CollisionUpdators
{
    public partial struct DestroyableCollisionUpdator_System : ISystem, IEnableableSystem
    {
        public static DestroyableCollisionUpdator_System instance { get; private set; }
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

            foreach (var (destroyableComponent, destroyableTransform) in SystemAPI.Query<RefRW<Destroyable_ComponentData>, RefRW<LocalTransform>>())
            {
                if (destroyableComponent.ValueRO.toDestroy == true) continue;

                if (destroyableTransform.ValueRW.Position.y <= 0.55)
                {
                    destroyableComponent.ValueRW.toDestroy = true;
                }
            }

            foreach (var (picturePieceComponent, picturePiecesTransform) in SystemAPI.Query<RefRW<PicturePiece_ComponentData>, RefRW<LocalTransform>>())
            {
                if (picturePieceComponent.ValueRO.isHit == true)
                {
                    if (picturePiecesTransform.ValueRW.Position.y >= 1.1)
                    {
                        continue;
                    }

                    if (picturePiecesTransform.ValueRW.Scale <= 0.015f)
                    {
                        continue;
                    }

                    picturePiecesTransform.ValueRW.Scale -= 0.01f;
                }
            }
        }
    }
}