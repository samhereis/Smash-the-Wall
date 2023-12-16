using ECS.ComponentData.Other;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.CollisionUpdators
{
    public struct DestroyOnCollided<T_ComponentToCheckFor> : ICollisionEventsJob where T_ComponentToCheckFor : unmanaged, IComponentData
    {
        public ComponentLookup<Destroyable_ComponentData> collisionEventDestroyData;

        private EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

        public void Execute(CollisionEvent collisionEvent)
        {
            /*var entity = collisionEvent.EntityA;

            if (entityManager.HasComponent<Destroyable_ComponentData>(entity) == false)
            {
                return;
            }

            var collidedWith = collisionEvent.EntityB;

            if (entityManager.HasComponent<T_ComponentToCheckFor>(collidedWith))
            {
                var component = collisionEventDestroyData[entity];
                component.toDestroy = true;
                collisionEventDestroyData[entity] = component;
            }*/
        }
    }
}