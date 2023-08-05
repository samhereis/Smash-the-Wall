using ECS.ComponentData.Other;
using ECS.ComponentData.Projectile;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class BulletAuthoring : MonoBehaviour
    {
        [field: SerializeField] public float force { get; private set; } = 100;
        [field: SerializeField] public Destroyable_ComponentData destroyable_ComponentData { get; private set; }

        public class BulletBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Destroyable_ComponentData
                {
                    toDestroy = authoring.destroyable_ComponentData.toDestroy
                });

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Projectile_ComponentData());
            }
        }
    }
}