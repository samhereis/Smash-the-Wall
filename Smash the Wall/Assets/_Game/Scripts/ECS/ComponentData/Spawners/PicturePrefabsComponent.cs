using Unity.Entities;

namespace ECS.ComponentData
{
    public struct PicturePrefabsComponent : IComponentData
    {

    }

    public struct PicturePrefabData : IComponentData
    {
        public Entity prefab;
    }

    public struct PicturePrefabBufferElement : IBufferElementData
    {
        public PicturePrefabData value;
    }
}