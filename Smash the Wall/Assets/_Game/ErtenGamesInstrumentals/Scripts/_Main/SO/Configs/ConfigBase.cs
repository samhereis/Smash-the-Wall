using Interfaces;
using UnityEngine;

namespace Configs
{
    public abstract class ConfigBase : ScriptableObject, IInitializable
    {
        public virtual void Initialize() { }
    }
}