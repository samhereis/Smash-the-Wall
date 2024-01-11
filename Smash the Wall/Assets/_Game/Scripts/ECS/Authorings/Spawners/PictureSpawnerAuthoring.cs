using DependencyInjection;
using ECS.ComponentData;
using Helpers;
using Sirenix.OdinInspector;
using SO.Lists;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class PictureSpawnerAuthoring : MonoBehaviour, INeedDependencyInjection, ISelfValidator
    {
        [Required]
        [SerializeField] private ListOfAllPictures _listOfAllPictures;

#if UNITY_EDITOR
        [field: SerializeField] public List<PictureAuthoring> pictures = new List<PictureAuthoring>();
#endif

        public void Validate(SelfValidationResult result)
        {
#if UNITY_EDITOR
            foreach (var pictureInList in _listOfAllPictures.pictures) { pictures.SafeAdd(pictureInList.GetAsset()); }
#endif
        }

        public class PictureSpawnerAuthoring_Baker : Baker<PictureSpawnerAuthoring>
        {
            public override void Bake(PictureSpawnerAuthoring authoring)
            {
                var picturePrefabBuffer = AddBuffer<PicturePrefabBufferElement>(GetEntity(TransformUsageFlags.Dynamic));

#if UNITY_EDITOR
                foreach (var picture in authoring.pictures)
                {
                    picturePrefabBuffer.Add(new PicturePrefabBufferElement
                    {
                        value = new PicturePrefabData
                        {
                            prefab = GetEntity(picture, TransformUsageFlags.Dynamic)
                        }
                    });
                }
#endif

                AddComponent(GetEntity(TransformUsageFlags.None), new PicturePrefabsComponent());
            }
        }
    }
}