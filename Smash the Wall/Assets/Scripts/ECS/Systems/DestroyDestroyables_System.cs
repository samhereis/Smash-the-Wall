using ECS.ComponentData.Other;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems
{
    public partial class DestroyDestroyables_System : SystemBase, IEnableableSystem
    {
        public static DestroyDestroyables_System instance { get; private set; }
        public static bool _isActive { get; private set; }
        public bool isActive => _isActive;

        public void Enable()
        {
            Enabled = true;
            _isActive = true;
        }

        public void Disable()
        {
            Enabled = false;
            _isActive = false;
        }

        protected override void OnCreate()
        {
            instance = this;
            Disable();
        }

        protected override void OnUpdate()
        {
            if (isActive == false) return;

            using (var commandBuffer = new EntityCommandBuffer(Allocator.TempJob))
            {
                foreach (var (destroyer, localPosition, entity) in SystemAPI.Query<RefRW<Destroyable_ComponentData>, RefRW<LocalTransform>>().WithEntityAccess())
                {
                    if (destroyer.ValueRW.toDestroy || localPosition.ValueRW.Position.y < -500)
                    {
                        commandBuffer.DestroyEntity(entity);
                    }
                }

                commandBuffer.Playback(EntityManager);
            }
        }
    }
}