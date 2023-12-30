using DependencyInjection;
using Helpers;
using Interfaces;
using System;
using UnityEngine;

namespace GameState
{
    [Serializable]
    public class SimpleGameStatesChanger : IGameStateChanger, IInitializable, IDIDependent
    {
        [field: SerializeField] public GameStateBase currentGameState { get; private set; }

        public virtual async void Initialize()
        {
            while (DependencyInjector.isGloballyInjected == false)
            {
                await AsyncHelper.DelayFloat(1f);
            }

            DependencyInjector.InjectDependencies(this);
        }

        public virtual void ChangeState(GameStateBase gameState)
        {
            currentGameState?.Exit();

            currentGameState = gameState;
            currentGameState.Enter();
        }
    }
}