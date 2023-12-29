using Interfaces;
using System;

namespace GameState
{
    public abstract class GameState_EnemiesManagerBase<TSceneManager> : IInitializable where TSceneManager : GameState_SceneManagerBase
    {
        public Action<IDamagable> onEnemyKilled;

        protected TSceneManager _sceneManager;

        public GameState_EnemiesManagerBase(TSceneManager eFH_SceneManager)
        {
            _sceneManager = eFH_SceneManager;
        }

        public virtual void Initialize()
        {

        }

        public virtual void SubscribeToEvents()
        {

        }

        public virtual void UnsubscribeFromEvents()
        {

        }

        protected virtual void OnEnemyDied(IDamagable damagable)
        {
            onEnemyKilled?.Invoke(damagable);
        }
    }
}