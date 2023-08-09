using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Helpers
{
    [DisallowMultipleComponent]
    public class GraphicHelper : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> _meshRenderers;

        [ContextMenu(nameof(FindAllMeshRenderers))]
        public void FindAllMeshRenderers()
        {
            _meshRenderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
            this.TrySetDirty();
        }

        [ContextMenu(nameof(DisableAllShadows))]
        public async void DisableAllShadows()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                meshRenderer.TrySetDirty();
                await AsyncHelper.Delay();
            }
        }

        [ContextMenu(nameof(DisableAllRecieveShadows))]
        public async void DisableAllRecieveShadows()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.receiveShadows = false;
                meshRenderer.TrySetDirty();
                await AsyncHelper.Delay();
            }
        }
    }
}