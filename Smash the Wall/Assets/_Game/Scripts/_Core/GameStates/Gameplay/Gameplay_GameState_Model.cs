using ECS.Systems.GameState;
using InGameStrings;
using Observables;

namespace GameState
{
    public class Gameplay_GameState_Model : GameState_ModelBase
    {
        public enum GameplayState { unset, Gameplay, Pause, Win, Lose }

        public DataSignal<GameplayState> onGameplayStatusChanged = new DataSignal<GameplayState>(Event_DIStrings.onGameplayStatucChanged);

        public float releasedWhatNeedsToStaysPercentage => WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;
        public float releasedWhatNeedsToBeDestroysPercentage => WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage;

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }
    }
}