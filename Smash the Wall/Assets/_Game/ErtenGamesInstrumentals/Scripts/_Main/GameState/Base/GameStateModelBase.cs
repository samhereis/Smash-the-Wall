using Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameState
{
    public abstract class GameStateModelBase : MonoBehaviour, IInitializable, IDisposable
    {
        [field: SerializeField, ReadOnly] public bool isInitialized { get; protected set; }

#if UNITY_EDITOR
        [SerializeField] public bool isDebugMode { get; private set; } = false;
#else
        public bool isDebugMode => false;
#endif

        public virtual void Initialize()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}