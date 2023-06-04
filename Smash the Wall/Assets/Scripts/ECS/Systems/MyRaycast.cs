using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using RaycastHit = Unity.Physics.RaycastHit;

namespace ECS.Systems
{
    public partial class MyRaycast : SystemBase
    {
        public static MyRaycast instance;

        protected override void OnCreate()
        {
            instance = this;
        }

        protected override void OnUpdate()
        {

        }

        public Entity Raycast(float3 from, float3 to)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();

            EntityQuery singleQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singleQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singleQuery.Dispose();

            RaycastInput input = new RaycastInput()
            {
                Start = from,
                End = to,
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u,
                    GroupIndex = 0
                }
            };

            RaycastHit hit = new RaycastHit();
            bool hasHit = collisionWorld.CastRay(input, out hit);

            if (hasHit)
            {
                return hit.Entity;
            }
            else
            {
                return Entity.Null;
            }
        }

        public static List<RaycastHit> RaycastAll(float3 from, float3 to)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();

            EntityQuery singleQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singleQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singleQuery.Dispose();

            RaycastInput input = new RaycastInput()
            {
                Start = from,
                End = to,
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u,
                    GroupIndex = 0
                }
            };

            NativeList<RaycastHit> hit = new NativeList<RaycastHit>();
            collisionWorld.CastRay(input, ref hit);

            var result = new List<RaycastHit>();
            result.AddRange(hit);

            return result;
        }
    }
}