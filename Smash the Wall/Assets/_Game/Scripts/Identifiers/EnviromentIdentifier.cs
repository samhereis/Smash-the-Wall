using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Identifiers
{
    public sealed class EnviromentIdentifier : IdentifierBase, IInitializable
    {
        [Required]
        [SerializeField] private Material _skyBox;

        public void Initialize()
        {
            if (_skyBox != null)
            {
                RenderSettings.skybox = _skyBox;
            }
        }
    }
}