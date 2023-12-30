using Interfaces;
using System;

namespace GameState
{
    public abstract class EnemiesManagerBase : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {

        }
        public virtual void Dispose()
        {

        }
    }
}