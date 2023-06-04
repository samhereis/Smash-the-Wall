using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Helpers
{
    public class GraphicHelper : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> _meshRenderers;
        public LayerMask _layerMask = 0;

        [ContextMenu(nameof(FindAllMeshRenderers))]
        public void FindAllMeshRenderers()
        {
            _meshRenderers = FindObjectsOfType<MeshRenderer>().ToList();
        }

        [ContextMenu(nameof(DisableAllShadows))]
        public async void DisableAllShadows()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                await AsyncHelper.Delay();
            }
        }

        [ContextMenu(nameof(DisableAllRecieveShadows))]
        public async void DisableAllRecieveShadows()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.receiveShadows = false;
                await AsyncHelper.Delay();
            }
        }

        private void RenameLater()
        {

#if UNITY_EDITOR

            _layerMask = 11;

            _meshRenderers.Clear();
            foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>(true))
            {
                if (meshRenderer.gameObject.name != "coin janga") _meshRenderers.Add(meshRenderer);
            }

            foreach (var meshRenderer in _meshRenderers)
            {
                var newFlags = StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.ReflectionProbeStatic;
                GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, newFlags);
                meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            }

            foreach (var lod in GetComponentsInChildren<LODGroup>(true))
            {
                try
                {
                    if (lod.gameObject.name != "coin janga")
                    {
                        foreach (var lodChild in lod.GetLODs())
                        {
                            foreach (var renderer in lodChild.renderers)
                            {
                                renderer.transform.localPosition = Vector3.zero;
                            }

                            lod.size *= 2;
                            if (lod.size > 75) lod.size = 75;
                        }
                    }
                    else
                    {
                        foreach (var lodChild in lod.GetLODs())
                        {
                            foreach (var renderer in lodChild.renderers)
                            {
                                renderer.transform.localPosition = Vector3.zero;
                            }

                            lod.size *= 2;
                            if (lod.size > 10) lod.size = 10;
                        }
                    }
                }
                finally
                {

                }
            }
#endif

            this.TrySetDirty();
        }
    }
}