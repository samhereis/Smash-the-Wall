using Backend;
using Configs;
using DI;
using ECS.ComponentData;
using ECS.ComponentData.Other;
using ECS.ComponentData.Picture.Piece;
using SO.Lists;
using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems.Spawners
{
    public partial struct PictureSpawner_System : ISystem, IEnableableSystem
    {
        public static PictureSpawner_System instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

        private static ListOfAllPictures _listOfAllPictures;
        private static GameConfigs _gameConfigs;

        public void Enable()
        {
            _isActive = true;

            _listOfAllPictures = DIBox.Get<ListOfAllPictures>(InGameStrings.DIStrings.listOfAllPictures);
            _gameConfigs = DIBox.Get<GameConfigs>(InGameStrings.DIStrings.gameConfigs);
        }

        public void Disable()
        {
            _isActive = false;
        }

        public void OnCreate(ref SystemState state)
        {
            instance = this;
            Disable();
        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            if (isActive == false) return;

            SpawnAllPictures(ref state);
            AutoManageChildren(ref state);
        }

        private void SpawnAllPictures(ref SystemState state)
        {
            foreach (var (pictureSpawner, entity) in SystemAPI.Query<RefRW<PictureSpawner_ComponentData>>().WithEntityAccess())
            {
                if (entityManager.HasBuffer<PictureSpawnBufferElement>(entity) == false) continue;
                if (entityManager.HasBuffer<PicturePrefabBufferElement>(entity) == false) continue;
                if (pictureSpawner.ValueRW.hasSpawnAll == true) continue;

                var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
                var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

                int numberOfSpawnedPictures = 0;

                try
                {
                    var pictureSpawnBuffer = entityManager.GetBuffer<PictureSpawnBufferElement>(entity);
                    var picturePrefabBuffer = entityManager.GetBuffer<PicturePrefabBufferElement>(entity);

                    foreach (var picture in pictureSpawnBuffer)
                    {
                        var newPicture = Entity.Null;

                        if (_gameConfigs.isRestart == false)
                        {
                            if (GameConfigs.GameSettings.isRamdonPicturesEnabled)
                            {
                                newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GetRandomIndex()].value.prefab);
                            }
                            else
                            {
                                newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GeCurrentIndex()].value.prefab);
                            }
                        }
                        else
                        {
                            newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GeCurrentIndex()].value.prefab);
                        }

                        Debug.Log($"Spawned {entityManager.GetName(entity)} at {picture.value.position}");

                        var localTransformComponent = new LocalTransform
                        {
                            Position = picture.value.position,
                            Rotation = picture.value.rotation,
                            Scale = 2
                        };

                        ecb.AddComponent<LocalTransform>(newPicture, localTransformComponent);
                        ecb.SetComponent<LocalTransform>(newPicture, localTransformComponent);

                        numberOfSpawnedPictures++;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                finally
                {
                    if (pictureSpawner.ValueRO.numberOfSpawnablePictures > 0)
                    {
                        if (numberOfSpawnedPictures == pictureSpawner.ValueRO.numberOfSpawnablePictures)
                        {
                            pictureSpawner.ValueRW.hasSpawnAll = true;
                        }
                    }
                }
            }
        }

        private void AutoManageChildren(ref SystemState systemState)
        {
            foreach (var (parent, localToWorldTransforms, entity) in SystemAPI.Query
                <
                RefRW<Parent_ComponentData>,
                RefRW<LocalToWorld>
                >().WithEntityAccess())
            {
                if (parent.ValueRO.isPositionHandled == true) continue;
                if (SystemAPI.HasBuffer<LinkedEntityGroup>(entity) == false) continue;

                var buffer = SystemAPI.GetBuffer<LinkedEntityGroup>(entity);
                int bufferCount = buffer.Length;
                int numberOfHandledChildren = 0;

                foreach (var child in buffer)
                {
                    if (entityManager.HasComponent<PicturePiece_ComponentData>(child.Value) == false) continue;
                    if (SystemAPI.GetComponent<PicturePiece_ComponentData>(child.Value).isKinematic == false) continue;
                    if (SystemAPI.HasComponent<ChildTransform_ComponentData>(child.Value) == false) continue;

                    var childTransform_ComponentData = SystemAPI.GetComponent<ChildTransform_ComponentData>(child.Value);

                    var localTransform = SystemAPI.GetComponent<LocalTransform>(child.Value);
                    localTransform.Position = localToWorldTransforms.ValueRW.Position + childTransform_ComponentData.localPosition;
                    localTransform.RotateX(childTransform_ComponentData.rotation.x);
                    localTransform.RotateY(childTransform_ComponentData.rotation.y);
                    localTransform.RotateZ(childTransform_ComponentData.rotation.z);

                    SystemAPI.SetComponent<LocalTransform>(child.Value, localTransform);

                    numberOfHandledChildren++;
                }

                if (numberOfHandledChildren == bufferCount) parent.ValueRW.isPositionHandled = true;
            }
        }
    }
}