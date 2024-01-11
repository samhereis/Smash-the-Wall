using Configs;
using DependencyInjection;
using ECS.Systems.GameState;
using InGameStrings;
using Observables;

namespace GameState
{
    public class Gameplay_GameState_Model : GameState_ModelBase, INeedDependencyInjection, IWinStarCalculator
    {
        public enum GameplayState { unset, Gameplay, Pause, Win, Lose }

        public DataSignal<GameplayState> onGameplayStatusChanged = new DataSignal<GameplayState>(Event_DIStrings.onGameplayStatucChanged);

        public float releasedWhatNeedsToStaysPercentage => WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;
        public float releasedWhatNeedsToBeDestroysPercentage => WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage;

        [Inject] private GameConfigs _gameConfigs;

        public override void Initialize()
        {
            base.Initialize();

            DependencyContext.diBox.InjectDataTo(this);

            isInitialized = true;
        }

        public int CalculateWinStars()
        {
            int stars = 0;
            float currentPercentage = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;

            for (int i = 0; i < _gameConfigs.gameSettings.winLoseStarSettings.Count; i++)
            {
                float percentage = _gameConfigs.gameSettings.winLoseStarSettings[i].percentage;

                if (currentPercentage >= percentage)
                {
                    stars = i + 1;
                }
            }

            return stars;
        }
    }
}