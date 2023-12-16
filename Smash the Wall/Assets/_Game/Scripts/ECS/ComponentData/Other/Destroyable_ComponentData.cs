using System;
using Unity.Entities;

namespace ECS.ComponentData.Other
{
    [Serializable]
    public struct Destroyable_ComponentData : IComponentData
    {
        public bool toDestroy;
        public bool isDestroyed;
    }
}