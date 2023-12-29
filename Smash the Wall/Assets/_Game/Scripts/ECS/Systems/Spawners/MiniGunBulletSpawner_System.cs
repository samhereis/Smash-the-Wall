using ECS.ComponentData;
using Interfaces;
using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Weapons;

namespace ECS.Systems.Spawners
{
    public partial struct ProjectileiGunBulletSpawner_System : ISystem, IEnableableSystem, IInitializable<ProjectileWeaponBase>, IDisposable
    {
        public static ProjectileiGunBulletSpawner_System instance { get; private set; }
        public static bool isCurrentlyActive { get; private set; }
        public bool isActive => isCurrentlyActive;

        private static ProjectileWeaponBase _projectileWeapon;

        public void Initialize(ProjectileWeaponBase projectileWeapon)
        {
            _projectileWeapon = projectileWeapon;
        }

        public void Dispose()
        {
            _projectileWeapon = null;
        }

        public void Enable()
        {
            if (_projectileWeapon != null)
            {
                isCurrentlyActive = true;
            }
        }

        public void Disable()
        {
            isCurrentlyActive = false;
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
            if (_projectileWeapon == null) return;

            if (_projectileWeapon.canShoot)
            {
                var spawnPhysics = SystemAPI.GetSingleton<ProjectileToShoot_ComponentData>();
                var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
                var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

                var bulletEntity = ecb.Instantiate(spawnPhysics.bulletPrefab);

                var shootPosition = _projectileWeapon.shootPosition;

                var localTransformComponent = new LocalTransform
                {
                    Position = new float3(shootPosition.position.x, shootPosition.position.y, shootPosition.position.z),
                    Rotation = new quaternion(shootPosition.rotation.x, shootPosition.rotation.y, shootPosition.rotation.z, shootPosition.rotation.w),
                    Scale = 1
                };

                ecb.SetComponent(bulletEntity, localTransformComponent);

                ecb.SetComponent(bulletEntity, new PhysicsVelocity
                {
                    Linear = localTransformComponent.Forward() * spawnPhysics.force
                });

                _projectileWeapon?.OnFired();
            }
        }
    }
}