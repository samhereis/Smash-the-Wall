using ECS.ComponentData;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Weapons;

namespace ECS.Systems.Spawners
{
    [BurstCompile]
    public partial struct MiniGunBulletSpawner_System : ISystem, IEnableableSystem
    {
        public static MiniGunBulletSpawner_System instance { get; private set; }
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

        public void OnCreate(ref SystemState state)
        {
            instance = this;

            state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadWrite<ProjectileToShoot_ComponentData>()));

            Disable();
        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            if (isActive == false) return;

            if (MiniGun.canShoot)
            {
                var spawnPhysics = SystemAPI.GetSingleton<ProjectileToShoot_ComponentData>();
                var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
                var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

                var bulletEntity = ecb.Instantiate(spawnPhysics.bulletPrefab);

                var localTransformComponent = new LocalTransform
                {
                    Position = new float3(MiniGun.shootPosition.position.x, MiniGun.shootPosition.position.y, MiniGun.shootPosition.position.z),
                    Rotation = new quaternion(MiniGun.shootPosition.rotation.x, MiniGun.shootPosition.rotation.y, MiniGun.shootPosition.rotation.z, MiniGun.shootPosition.rotation.w),
                    Scale = 1
                };

                ecb.SetComponent(bulletEntity, localTransformComponent);

                ecb.SetComponent(bulletEntity, new PhysicsVelocity
                {
                    Linear = localTransformComponent.Forward() * spawnPhysics.force
                });
            }
        }
    }
}