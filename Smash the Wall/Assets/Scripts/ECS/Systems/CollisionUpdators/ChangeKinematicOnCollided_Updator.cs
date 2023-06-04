using ECS.CollisionEventJobs;
using ECS.ComponentData.Picture.Piece;
using ECS.ComponentData.Projectile;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.CollisionUpdators
{
    public partial struct ChangeKinematicOnCollided_Updator : ISystem, IEnableableSystem
    {
        public static ChangeKinematicOnCollided_Updator instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public ComponentLookup<PicturePiece_ComponentData> collisionEventDestroyData;

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

            systemState.RequireForUpdate(systemState.GetEntityQuery(ComponentType.ReadWrite<PicturePiece_ComponentData>()));
            collisionEventDestroyData = systemState.GetComponentLookup<PicturePiece_ComponentData>(false);

            Disable();
        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            collisionEventDestroyData.Update(ref systemState);
            var job = new ChangeKinematicOnCollided_CollisionEventsJob<Projectile_ComponentData>();
            job.collisionEventDestroyData = collisionEventDestroyData;

            var jobHandler = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), systemState.Dependency);

            systemState.Dependency = jobHandler;
        }
    }
}