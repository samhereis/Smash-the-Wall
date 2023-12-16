using System;
using Unity.Entities;

namespace ECS.ComponentData
{
    [Serializable]
    public struct ProjectileToShoot_ComponentData : IComponentData
    {
        public Entity bulletPrefab;
        public float force;
    }
}