using ECS.ComponentData.Picture.Piece;
using Unity.Entities;
using Unity.Physics;

namespace ECS.CollisionEventJobs
{
    public struct ChangeKinematicOnCollided_CollisionEventsJob<T_ComponentToCheckFor> : ICollisionEventsJob where T_ComponentToCheckFor : unmanaged, IComponentData
    {
        public ComponentLookup<PicturePiece_ComponentData> collisionEventDestroyData;

        private EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

        public void Execute(CollisionEvent collisionEvent)
        {
            /*var entity = collisionEvent.EntityA;

            if (entityManager.HasComponent<PicturePiece_ComponentData>(entity) == false)
            {
                return;
            }

            var collidedWith = collisionEvent.EntityB;

            if (entityManager.HasComponent<T_ComponentToCheckFor>(collidedWith))
            {
                var component = collisionEventDestroyData[entity];
                component.isHit = true;
                collisionEventDestroyData[entity] = component;
            }*/
        }
    }
}