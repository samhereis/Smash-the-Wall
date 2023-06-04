using ECS.ComponentData.Enviroment;
using ECS.ComponentData.Other;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.CollisionUpdators
{
    public partial struct DestroyableCollisionUpdator_System : ISystem, IEnableableSystem
    {
        public static DestroyableCollisionUpdator_System instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public ComponentLookup<Destroyable_ComponentData> collisionEventDestroyData;

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

            systemState.RequireForUpdate(systemState.GetEntityQuery(ComponentType.ReadWrite<Destroyable_ComponentData>()));
            collisionEventDestroyData = systemState.GetComponentLookup<Destroyable_ComponentData>(false);

            Disable();
        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            collisionEventDestroyData.Update(ref systemState);
            var job = new DestroyOnCollided<Ground_ComponentData>();
            job.collisionEventDestroyData = collisionEventDestroyData;

            var jobHandler = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), systemState.Dependency);

            systemState.Dependency = jobHandler;
        }
    }
}