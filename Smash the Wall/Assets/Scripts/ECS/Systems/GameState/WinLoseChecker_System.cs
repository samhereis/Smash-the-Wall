using Configs;
using DI;
using ECS.ComponentData.Picture.Piece;
using Events;
using Helpers;
using InGameStrings;
using Unity.Entities;

namespace ECS.Systems.GameState
{
    public partial struct WinLoseChecker_System : ISystem, IEnableableSystem, IDIDependent
    {
        public static WinLoseChecker_System instance { get; private set; }
        public static bool _isActive { get; private set; }

        public static float releasedWhatNeedsToBeDestroysPercentage;
        public static float releasedWhatNeedsToStaysPercentage;

        private static EventWithNoParameters onWin;
        private static EventWithNoParameters onLose;
        private static GameConfigs gameConfigs;

        public bool isActive => _isActive;

        public void Enable()
        {
            _isActive = true;

            InjectDs();
        }

        public void Disable()
        {
            _isActive = false;
        }

        public void OnCreate(ref SystemState systemState)
        {
            instance = this;
            Disable();
        }

        private void InjectDs()
        {
            onWin = DIBox.Get<EventWithNoParameters>(DIStrings.onWinEvent);
            onLose = DIBox.Get<EventWithNoParameters>(DIStrings.onLoseEvent);
            gameConfigs = DIBox.Get<GameConfigs>(DIStrings.gameConfigs);
        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;

            if (HasWon(ref systemState))
            {
                onWin?.Invoke();
                Disable();
            }
            else if (HasLost(ref systemState))
            {
                onLose?.Invoke();
                Disable();
            }
        }

        private bool HasWon(ref SystemState systemState)
        {
            if (gameConfigs == null)
            {
                InjectDs();
                return false;
            }

            int numberOfAllWhatNeedsToBeDestroys = 0;
            int numberOfAllReleasedWhatNeedsToBeDestroys = 0;

            foreach (var (whatNeedsToBeDestroyed, picturePiece, entity) in SystemAPI.Query
                <
                    RefRW<WhatNeedsToBeDestroyed_ComponentData>,
                    RefRW<PicturePiece_ComponentData>
                >().WithEntityAccess())
            {
                numberOfAllWhatNeedsToBeDestroys++;
                if (picturePiece.ValueRO.isHit == true)
                {
                    numberOfAllReleasedWhatNeedsToBeDestroys++;
                }
            }

            if (numberOfAllWhatNeedsToBeDestroys != 0)
            {
                releasedWhatNeedsToBeDestroysPercentage = NumberHelper.GetPercentageOf100(numberOfAllReleasedWhatNeedsToBeDestroys, numberOfAllWhatNeedsToBeDestroys);

                if (releasedWhatNeedsToBeDestroysPercentage >= gameConfigs.gameplaySettings.percentageOfReleasedWhatNeedsToBeDestroysToWin)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasLost(ref SystemState systemState)
        {
            if (gameConfigs == null)
            {
                InjectDs();
                return false;
            }

            int numberOfAllWhatNeedsToStays = 0;
            int numberOfAllReleasedWhatNeedsToStays = 0;

            foreach (var (whatNeedsToStay, picturePiece, entity) in SystemAPI.Query
                <
                    RefRW<WhatNeedsToStay_ComponentData>,
                    RefRW<PicturePiece_ComponentData>
                >().WithEntityAccess())
            {
                numberOfAllWhatNeedsToStays++;
                if (picturePiece.ValueRO.isHit == true)
                {
                    numberOfAllReleasedWhatNeedsToStays++;
                }
            }

            if (numberOfAllWhatNeedsToStays != 0)
            {
                releasedWhatNeedsToStaysPercentage = 100 - NumberHelper.GetPercentageOf100(numberOfAllReleasedWhatNeedsToStays, numberOfAllWhatNeedsToStays);

                if (releasedWhatNeedsToStaysPercentage <= gameConfigs.gameplaySettings.percentageOfReleasedWhatNeedsToStaysToLose)
                {
                    return true;
                }
            }

            return false;
        }
    }
}