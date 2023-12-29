using ECS.ComponentData;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class ProjectileBulletSpawner_Authoring : MonoBehaviour
    {
        [Required]
        public BulletAuthoring bulletPrefab;

        public class ShootBulletBaker : Baker<ProjectileBulletSpawner_Authoring>
        {
            public override void Bake(ProjectileBulletSpawner_Authoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ProjectileToShoot_ComponentData
                {
                    bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                    force = authoring.bulletPrefab.force
                });
            }
        }
    }
}