using DependencyInjection;
using ECS.ComponentData;
using Identifiers;
using Sirenix.OdinInspector;
using SO.Lists;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring
{
    public class PictureSpawnerAuthoring : MonoBehaviour, INeedDependencyInjection
    {
        public struct PictureSpawnData_GameObject
        {
            public float3 position;
            public quaternion rotation;
        }

        public class PictureSpawnerAuthoring_Baker : Baker<PictureSpawnerAuthoring>
        {
            public override void Bake(PictureSpawnerAuthoring authoring)
            {
                var pictureSpawnBuffer = AddBuffer<PictureSpawnBufferElement>(GetEntity(TransformUsageFlags.Dynamic));

                var allPicturesSpawnPositions = authoring.GetPictures();

                foreach (var picture in allPicturesSpawnPositions)
                {
                    pictureSpawnBuffer.Add(new PictureSpawnBufferElement
                    {
                        value = new PictureSpawnData
                        {
                            position = picture.position,
                            rotation = picture.rotation
                        }
                    });
                }

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

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PictureSpawner_ComponentData
                {
                    numberOfSpawnablePictures = allPicturesSpawnPositions.Count
                });
            }
        }

        [Required]
        [field: SerializeField] public ListOfAllPictures pictures { get; private set; }

        [Required]
        [SerializeField] private PicturePlacesIdentifier _picturePlaces;

        public List<PictureSpawnData_GameObject> GetPictures()
        {
            var picturesList = new List<PictureSpawnData_GameObject>();

            if (_picturePlaces == null) { _picturePlaces = FindObjectOfType<PicturePlacesIdentifier>(true); }

            foreach (var picturePlace in _picturePlaces.picturePlaces)
            {
                var pictureSpawnData = new PictureSpawnData_GameObject
                {
                    position = new float3(picturePlace.position.x, picturePlace.position.y, picturePlace.position.z),
                    rotation = picturePlace.rotation
                };

                picturesList.Add(pictureSpawnData);
            }

            return picturesList;
        }
    }
}