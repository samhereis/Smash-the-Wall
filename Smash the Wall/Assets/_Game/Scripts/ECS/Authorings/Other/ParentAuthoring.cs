using ECS.ComponentData.Other;
using Unity.Entities;
using Unity.Entities.Hybrid.Baking;
using UnityEngine;

namespace ECS.Authoring
{
    [RequireComponent(typeof(LinkedEntityGroupAuthoring))]
    public class ParentAuthoring : MonoBehaviour
    {
        public class ParentBaker : Baker<ParentAuthoring>
        {
            public override void Bake(ParentAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Parent_ComponentData());
            }
        }
    }
}