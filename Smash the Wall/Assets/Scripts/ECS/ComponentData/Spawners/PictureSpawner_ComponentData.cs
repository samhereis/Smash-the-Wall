using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ComponentData
{
    public struct PictureSpawner_ComponentData : IComponentData
    {
        public int numberOfSpawnablePictures;
        public bool hasSpawnAll;
    }

    public struct PictureSpawnData : IBufferElementData
    {
        public float3 position;
        public quaternion rotation;
    }

    public struct PicturePrefabData : IComponentData
    {
        public Entity prefab;
    }

    public struct PicturePrefabBufferElement : IBufferElementData
    {
        public PicturePrefabData value;
    }

    public struct PictureSpawnBufferElement : IBufferElementData
    {
        public PictureSpawnData value;
        public bool hasSpawned;
    }
}