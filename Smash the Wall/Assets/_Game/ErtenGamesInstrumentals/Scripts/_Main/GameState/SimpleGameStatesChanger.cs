using DependencyInjection;
using Helpers;
using Interfaces;
using System;
using UnityEngine;

namespace GameState
{
    [Serializable]
    public class SimpleGameStatesChanger : IGameStateChanger, IInitializable, INeedDependencyInjection
    {
        [field: SerializeField] public GameState_ControllerBase currentGameState { get; private set; }

        public virtual async void Initialize()
        {
            while (DependencyContext.isGloballyInjected == false)
            {
                await AsyncHelper.DelayFloat(1f);
            }

            DependencyContext.InjectDependencies(this);
        }

        public virtual void ChangeState(GameState_ControllerBase gameState)
        {
            currentGameState?.Exit();

            currentGameState = gameState;
            currentGameState.Enter();
        }
    }
}