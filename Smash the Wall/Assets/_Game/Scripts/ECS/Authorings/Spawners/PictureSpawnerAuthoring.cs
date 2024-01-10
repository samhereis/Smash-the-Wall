using DependencyInjection;
using ECS.ComponentData;
using Sirenix.OdinInspector;
using SO.Lists;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class PictureSpawnerAuthoring : MonoBehaviour, INeedDependencyInjection
    {
        [Required]
        [field: SerializeField] public ListOfAllPictures pictures { get; private set; }

        public class PictureSpawnerAuthoring_Baker : Baker<PictureSpawnerAuthoring>
        {
            public override void Bake(PictureSpawnerAuthoring authoring)
            {
                var picturePrefabBuffer = AddBuffer<PicturePrefabBufferElement>(GetEntity(TransformUsageFlags.Dynamic));

                var allPicturesPrefabs = authoring.pictures.pictures;

                foreach (var picture in allPicturesPrefabs)
                {
                    picturePrefabBuffer.Add(new PicturePrefabBufferElement
                    {
                        value = new PicturePrefabData
                        {
                            prefab = GetEntity(picture.target, TransformUsageFlags.Dynamic)
                        }
                    });
                }

                AddComponent(GetEntity(TransformUsageFlags.None), new PicturePrefabsComponent());
            }
        }
    }
}