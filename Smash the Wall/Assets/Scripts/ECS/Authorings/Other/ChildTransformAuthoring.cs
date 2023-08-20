using ECS.ComponentData.Other;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring
{
    public class ChildTransformAuthoring : MonoBehaviour
    {
        public class ShootBulletBaker : Baker<ChildTransformAuthoring>
        {
            public override void Bake(ChildTransformAuthoring authoring)
            {
                authoring.Init();

                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ChildTransform_ComponentData
                {
                    localPosition = authoring.localPosition,
                    rotation = authoring.rotation
                });
            }
        }

        [field: SerializeField] public Vector3 localPosition { get; private set; }
        [field: SerializeField] public Quaternion rotation { get; private set; }

        [ContextMenu(nameof(Init))]
        public void Init()
        {
            localPosition = transform.position;
            rotation = transform.rotation;

            Debug.Log(rotation);
        }
    }
}