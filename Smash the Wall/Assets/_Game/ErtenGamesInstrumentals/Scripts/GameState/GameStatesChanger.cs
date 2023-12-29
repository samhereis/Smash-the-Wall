using DependencyInjection;
using Sirenix.OdinInspector;
using Helpers;
using Interfaces;

namespace GameState
{
    public class GameStatesChanger : IGameStateChanger, IInitializable, IDIDependent
    {
        [ShowInInspector] public GameStateBase currentGameState { get; private set; }

        public async void Initialize()
        {
            while (DependencyInjector.isGloballyInjected == false)
            {
                await AsyncHelper.DelayFloat(1f);
            }

            DependencyInjector.InjectDependencies(this);
        }

        public void ChangeState(GameStateBase gameState)
        {
            currentGameState?.Exit();

            currentGameState = gameState;
            currentGameState.Enter();
        }
    }
}