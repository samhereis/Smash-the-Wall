using ECS.ComponentData.Enviroment;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class GroundAuthoring : MonoBehaviour
    {
        public class GroundBaker : Baker<GroundAuthoring>
        {
            public override void Bake(GroundAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Renderable), new Ground_ComponentData());
            }
        }
    }
}