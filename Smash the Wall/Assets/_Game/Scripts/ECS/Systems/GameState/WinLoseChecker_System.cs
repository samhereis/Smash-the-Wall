using Configs;
using DependencyInjection;
using ECS.ComponentData.Picture.Piece;
using Events;
using Helpers;
using IdentityCards;
using InGameStrings;
using SO.Lists;
using Unity.Entities;

namespace ECS.Systems.GameState
{
    public partial struct WinLoseChecker_System : ISystem, IEnableableSystem, IDIDependent
    {
        public static WinLoseChecker_System instance { get; private set; }
        public static bool _isActive { get; private set; }

        public static float releasedWhatNeedsToBeDestroysPercentage;
        public static float releasedWhatNeedsToStaysPercentage;

        private static EventWithNoParameters _onWin;
        private static EventWithNoParameters _onLose;
        private static GameConfigs _gameConfigs;
        private static ListOfAllPictures _listOfAllPictures;

        private static PictureIdentityCard _currentPictureIdentityCard;

        public bool isActive => _isActive;

        public void Enable()
        {
            Clear();

            _isActive = true;

            InjectDs();

            _currentPictureIdentityCard = _listOfAllPictures.GetCurrent();
        }

        public void Disable()
        {
            _isActive = false;

            Clear();
        }

        public void OnCreate(ref SystemState systemState)
        {
            instance = this;
            Disable();
        }

        private void InjectDs()
        {
            _onWin = DependencyInjector.diBox.Get<EventWithNoParameters>(DIStrings.onWinEvent);
            _onLose = DependencyInjector.diBox.Get<EventWithNoParameters>(DIStrings.onLoseEvent);
            _gameConfigs = DependencyInjector.diBox.Get<GameConfigs>(DIStrings.gameConfigs);
            _listOfAllPictures = DependencyInjector.diBox.Get<ListOfAllPictures>(DIStrings.listOfAllPictures);
        }

        public void OnUpdate(ref SystemState systemState)
        {
            if (isActive == false) return;
            if (_currentPictureIdentityCard == null) { return; }

            switch (_currentPictureIdentityCard.pictureMode)
            {
                case DataClasses.Enums.PictureMode.DestroyBorder:
                    {
                        Update_DestroyBorderMode(ref systemState);

                        break;
                    }
                case DataClasses.Enums.PictureMode.DestroyWholeObject:
                    {
                        Update_DestroyWholeObjectMode(ref systemState);

                        break;
                    }
                case DataClasses.Enums.PictureMode.Coloring:
                    {
                        Update_ColoringMode(ref systemState);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Update_DestroyBorderMode(ref SystemState systemState)
        {
            if (HasWon(ref systemState))
            {
                _onWin?.Invoke();
                Disable();
            }
            else if (HasLost(ref systemState))
            {
                _onLose?.Invoke();
                Disable();
            }
        }

        private void Update_DestroyWholeObjectMode(ref SystemState systemState)
        {
            if (HasWon(ref systemState))
            {
                _onWin?.Invoke();
                Disable();
            }
        }

        private void Update_ColoringMode(ref SystemState systemState)
        {

        }

        private bool HasWon(ref SystemState systemState)
        {
            if (_gameConfigs == null)
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

                var percentageToWin = _gameConfigs.gameSettings.percentageOfReleasedWhatNeedsToBeDestroysToWin;

                if (percentageToWin < 50) percentageToWin = 85f;

                if (releasedWhatNeedsToBeDestroysPercentage >= percentageToWin)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasLost(ref SystemState systemState)
        {
            if (_gameConfigs == null)
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

                if (releasedWhatNeedsToStaysPercentage <= _gameConfigs.gameSettings.percentageOfReleasedWhatNeedsToStaysToLose)
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            releasedWhatNeedsToBeDestroysPercentage = 0;
            releasedWhatNeedsToStaysPercentage = 0;

            _currentPictureIdentityCard = null;
        }
    }
}