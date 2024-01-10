using Interfaces;
using System;

namespace GameState
{
    public abstract class GameState_ViewBase : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}