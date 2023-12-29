using Sirenix.OdinInspector;
using Interfaces;
using System;
using UnityEngine;

namespace GameState
{
    public abstract class GameState_SceneManagerBase : MonoBehaviour, IInitializable, IDisposable
    {
        [ShowInInspector] public bool isInitialized { get; protected set; }

#if UNITY_EDITOR
        [ShowInInspector] public bool isDebugMode { get; private set; } = false;
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