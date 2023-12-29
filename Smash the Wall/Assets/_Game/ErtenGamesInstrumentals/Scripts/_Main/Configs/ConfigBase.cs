using Interfaces;
using UnityEngine;

namespace Configs
{
    public abstract class ConfigBase : ScriptableObject, IInitializable
    {
        public abstract void Initialize();
    }
}