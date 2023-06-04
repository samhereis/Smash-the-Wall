using ECS.ComponentData.Picture.Piece;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace ECS.Systems
{
    public partial class CheckPicturePieceKinematic_System : SystemBase, IEnableableSystem
    {
        public static CheckPicturePieceKinematic_System instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public void Enable()
        {
            Enabled = true;
            _isActive = true;
        }

        public void Disable()
        {
            Enabled = false;
            _isActive = false;
        }

        protected override void OnCreate()
        {
            instance = this;
            Disable();
        }

        protected override void OnUpdate()
        {
            if (isActive == false) return;

            using (var commandBuffer = new EntityCommandBuffer(Allocator.TempJob))
            {
                foreach (var (picturePiece, physicsMassOverride, physicsVelocity, entity) in SystemAPI.Query
                    <
                        RefRW<PicturePiece_ComponentData>,
                        RefRW<PhysicsMassOverride>,
                        RefRW<PhysicsVelocity>
                    >().WithEntityAccess())
                {
                    if (picturePiece.ValueRW.isKinematic == true)
                    {
                        physicsMassOverride.ValueRW.IsKinematic = 1;
                        physicsVelocity.ValueRW.Linear = float3.zero;
                        physicsVelocity.ValueRW.Angular = float3.zero;
                    }
                    else
                    {
                        physicsMassOverride.ValueRW.IsKinematic = 0;
                    }
                }

                commandBuffer.Playback(EntityManager);
            }
        }
    }
}