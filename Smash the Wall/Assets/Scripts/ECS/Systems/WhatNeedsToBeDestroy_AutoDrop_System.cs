using ECS.ComponentData.Picture.Piece;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    [BurstCompile]
    public partial struct WhatNeedsToBeDestroy_AutoDrop_System : ISystem, IEnableableSystem
    {
        public static WhatNeedsToBeDestroy_AutoDrop_System instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public void Enable()
        {
            _isActive = true;
        }

        public void Disable()
        {
            _isActive = false;
        }

        public void OnCreate(ref SystemState systemState)
        {
            instance = this;
            Disable();
        }

        public void OnUpdate(ref SystemState systemState)
        {
            /*if (isActive == false) return;

            foreach (var (whatNeedsToBeDestroyed, picturePiece, entity) in SystemAPI.Query
                <
                    RefRW<WhatNeedsToBeDestroyed_ComponentData>,
                    RefRW<PicturePiece_ComponentData>
                >().WithEntityAccess())
            {
                TryReleasePiece(ref systemState, whatNeedsToBeDestroyed, picturePiece, entity);
            }*/
        }

        private void TryReleasePiece(ref SystemState systemState, RefRW<WhatNeedsToBeDestroyed_ComponentData> whatNeedsToBeDestroyed, RefRW<PicturePiece_ComponentData> picturePiece, Entity entity)
        {
            if (picturePiece.ValueRO.isHit == true)
            {
                var localToWorld = SystemAPI.GetComponent<LocalTransform>(entity);

                var startRay = localToWorld.Position;
                var endRay = startRay - new float3(0, 1f, 0);

                var hitEntity = MyRaycast.instance.Raycast(startRay, endRay);

                Debug.DrawRay(startRay, endRay);

                if (hitEntity == Entity.Null)
                {
                    picturePiece.ValueRW.isHit = true;
                }
            }
        }
    }
}