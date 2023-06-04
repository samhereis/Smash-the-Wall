using ECS.ComponentData;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class MiniGunBulletSpawner_Authoring : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float force;

        public class ShootBulletBaker : Baker<MiniGunBulletSpawner_Authoring>
        {
            public override void Bake(MiniGunBulletSpawner_Authoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ProjectileToShoot_ComponentData
                {
                    bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                    force = authoring.force
                });
            }
        }
    }
}