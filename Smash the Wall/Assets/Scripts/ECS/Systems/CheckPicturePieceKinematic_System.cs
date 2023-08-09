using ECS.ComponentData.Picture.Piece;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Aspects;

namespace ECS.Systems
{
    public partial struct CheckPicturePieceKinematic_System : ISystem, IEnableableSystem
    {
        public static CheckPicturePieceKinematic_System instance { get; private set; }
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

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            foreach (var (picturePiece, physicsMassOverride, ridigbody) in SystemAPI.Query
                <
                    RefRW<PicturePiece_ComponentData>,
                    RefRW<PhysicsMassOverride>,
                    RigidBodyAspect
                >())
            {
                if (picturePiece.ValueRW.isHit == true)
                {
                    ridigbody.IsKinematic = false;
                    physicsMassOverride.ValueRW.IsKinematic = 0;
                    physicsMassOverride.ValueRW.SetVelocityToZero = 0;
                }
                else if (picturePiece.ValueRW.isHit == false)
                {
                    ridigbody.IsKinematic = true;
                    physicsMassOverride.ValueRW.IsKinematic = 1;
                    physicsMassOverride.ValueRW.SetVelocityToZero = 1;
                }
            }
        }
    }
}