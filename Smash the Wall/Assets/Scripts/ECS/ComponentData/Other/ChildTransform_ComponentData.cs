using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ComponentData.Other
{
    public struct ChildTransform_ComponentData : IComponentData
    {
        public float3 localPosition;
        public float3 rotation;
    }
}