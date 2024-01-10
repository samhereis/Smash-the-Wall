using Configs;
using DependencyInjection;
using ECS.ComponentData;
using ECS.ComponentData.Other;
using ECS.ComponentData.Picture.Piece;
using Helpers;
using SO.Lists;
using Unity.Entities;
using Unity.Transforms;

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

            _listOfAllPictures = DependencyContext.diBox.Get<ListOfAllPictures>();
            _gameConfigs = DependencyContext.diBox.Get<GameConfigs>();
        }

        public void Disable()
        {
            _isActive = false;
        }

        private async void Inject()
        {

            while (DependencyContext.isGloballyInjected == false)
            {
                await AsyncHelper.Skip();
            }

            _listOfAllPictures = DependencyContext.diBox.Get<ListOfAllPictures>();
            _gameConfigs = DependencyContext.diBox.Get<GameConfigs>();
        }

        public void OnCreate(ref SystemState state)
        {
            instance = this;
            Disable();

            Inject();
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
            foreach (var (picturePrefabsComponent, entity) in SystemAPI.Query<RefRW<PicturePrefabsComponent>>().WithEntityAccess())
            {
                if (entityManager.HasBuffer<PicturePrefabBufferElement>(entity) == false) continue;

                var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
                var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

                var picturePrefabBuffer = entityManager.GetBuffer<PicturePrefabBufferElement>(entity);

                var newPicture = Entity.Null;

                if (_gameConfigs.isRestart == false)
                {
                    if (_gameConfigs.gameSettings.randomPictureSettings.currentValue == true)
                    {
                        newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GetRandomIndex()].value.prefab);
                    }
                    else
                    {
                        newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GetCurrentIndex()].value.prefab);
                    }
                }
                else
                {
                    newPicture = ecb.Instantiate(picturePrefabBuffer[_listOfAllPictures.GetCurrentIndex()].value.prefab);
                }

                if(newPicture != null)
                {
                    Disable();
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
                    if (SystemAPI.GetComponent<PicturePiece_ComponentData>(child.Value).isHit == true) continue;
                    if (SystemAPI.HasComponent<ChildTransform_ComponentData>(child.Value) == false) continue;

                    var childTransform_ComponentData = SystemAPI.GetComponent<ChildTransform_ComponentData>(child.Value);

                    var localTransform = SystemAPI.GetComponent<LocalTransform>(child.Value);
                    localTransform.Position = localToWorldTransforms.ValueRW.Position + childTransform_ComponentData.localPosition;
                    localTransform.Rotation = childTransform_ComponentData.rotation;

                    SystemAPI.SetComponent<LocalTransform>(child.Value, localTransform);

                    numberOfHandledChildren++;
                }

                if (numberOfHandledChildren == bufferCount)
                {
                    parent.ValueRW.isPositionHandled = true;
                    Disable();
                }
            }
        }
    }
}