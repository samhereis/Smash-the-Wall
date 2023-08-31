using Interfaces;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Scriptables/Config/GameConfigs")]
    public class GameConfigs : ConfigBase
    {
        [field: SerializeField, Header("In game temporary")] public bool isRestart { get; set; } = false;

        [field: SerializeField] public GameSettings gameSettings = new GameSettings();
        [field: SerializeField] public RateUsConfigs rateUsSettings = new RateUsConfigs();
        [field: SerializeField] public GameplayConfigs gameplaySettings = new GameplayConfigs();

        public override void Initialize()
        {
            gameplaySettings.Initialize();
            rateUsSettings.Initialize();
            gameSettings.Initialize();
        }

        [Serializable]
        public class GameSettings : IInitializable
        {
            [field: SerializeField] public BoolSavable_SO vibroSettings { get; private set; }
            [field: SerializeField] public BoolSavable_SO musicSettings { get; private set; }
            [field: SerializeField] public BoolSavable_SO soundsSettings { get; private set; }
            [field: SerializeField] public BoolSavable_SO randomEnviromentSettings { get; private set; }
            [field: SerializeField] public BoolSavable_SO randomPictureSettings { get; private set; }

            public void Initialize()
            {

            }

            public void SetVibroEnabled(bool isVibroEnabledNewValue)
            {
                vibroSettings.SetData(isVibroEnabledNewValue);
            }

            public void SetMusicEnabled(bool isMusicEnabledNewValue)
            {
                musicSettings.SetData(isMusicEnabledNewValue);
            }

            public void SetSoundsEnabled(bool areSoundsEnabledNewValue)
            {
                soundsSettings.SetData(areSoundsEnabledNewValue);
            }

            public void SetRamdonEnviromentEnabled(bool ramdonEnviromentNewValue)
            {
                randomEnviromentSettings.SetData(ramdonEnviromentNewValue);
            }

            public void SetRamdonPicturesEnabled(bool ramdonPicturesNewValue)
            {
                randomPictureSettings.SetData(ramdonPicturesNewValue);
            }
        }

        [Serializable]
        public class RateUsConfigs : IInitializable
        {
            private const string _hasRated = "hasRated";
            private const string _lastClickedOnLaterButtonLevel = "lastClickedOnLaterButtonLevel";

            [field: SerializeField] public string storeLink = "";

            [field: SerializeField] public int firstAppearOnLevel { get; private set; } = 2;
            [field: SerializeField] public int showLevelIncreaseValue { get; private set; } = 5;

            public void Initialize()
            {

            }

            public bool CanShow(int currentLevel)
            {
                currentLevel++;

                if (PlayerPrefs.GetInt(_hasRated, 0) == 0)
                {
                    if (currentLevel == firstAppearOnLevel)
                    {
                        return true;
                    }

                    int lastClickedOnLaterButtonLevel = PlayerPrefs.GetInt(_lastClickedOnLaterButtonLevel, 0);
                    if (currentLevel - lastClickedOnLaterButtonLevel == showLevelIncreaseValue)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void OnClickedLaterButton()
            {

            }

            public void OnRated()
            {

            }
        }

        [Serializable]
        public class GameplayConfigs : IInitializable
        {
            [field: SerializeField] public FloatSavable_SO gunRotationSpeed { get; private set; }

            [field: SerializeField] public float percentageOfReleasedWhatNeedsToBeDestroysToWin { get; private set; } = 90;
            [field: SerializeField] public float percentageOfReleasedWhatNeedsToStaysToLose { get; private set; } = 50;

            [field: SerializeField] public List<WinLoseStarSettings> winLoseStarSettings { get; private set; } = new List<WinLoseStarSettings>();

            [Serializable]
            public class WinLoseStarSettings
            {
                [field: SerializeField] public float percentage { get; private set; } = 0;
            }

            public void Initialize()
            {

            }

            public void SetGunRotationSpeed(float newGunRotationSpeed)
            {
                gunRotationSpeed.SetData(newGunRotationSpeed);
            }

            public void SetWinPercentage(float winPercantage)
            {
                percentageOfReleasedWhatNeedsToBeDestroysToWin = winPercantage;
            }

            public void SetLosePercentage(float losePercantage)
            {
                percentageOfReleasedWhatNeedsToStaysToLose = losePercantage;
            }
        }
    }
}