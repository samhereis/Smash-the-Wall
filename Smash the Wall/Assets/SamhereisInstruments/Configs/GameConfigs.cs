using Helpers;
using Interfaces;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Scriptables/Config/GameConfigs")]
    public class GameConfigs : ConfigBase
    {
        [field: SerializeField, Header("In game temporary")] public bool isRestart { get; set; } = false;

        [field: SerializeField] public GameSettings gameSettings = new GameSettings();
        [field: SerializeField] public RateUsConfigs rateUsSettings = new RateUsConfigs();

        public override void Initialize()
        {
            rateUsSettings.Initialize();
            gameSettings.Initialize();
        }

        [Serializable]
        public class GameSettings : IInitializable
        {
            [field: SerializeField, Header("Settings")] public FloatSavable_SO gunRotationSpeed { get; private set; }
            [field: SerializeField] public FloatSavable_SO musicValue { get; private set; }
            [field: SerializeField] public FloatSavable_SO soundsVolume { get; private set; }
            [field: SerializeField] public BoolSavable_SO vibroSettings { get; private set; }
            [field: SerializeField] public BoolSavable_SO randomPictureSettings { get; private set; }

            [field: SerializeField, Space(10)] public float percentageOfReleasedWhatNeedsToBeDestroysToWin { get; private set; } = 90;
            [field: SerializeField] public float percentageOfReleasedWhatNeedsToStaysToLose { get; private set; } = 50;

            [field: SerializeField, Header("Win Settings")] public List<WinLoseStarSettings> winLoseStarSettings { get; private set; } = new List<WinLoseStarSettings>();

            [field: SerializeField, Header("Audio Mixer")] public AudioMixer audioMixer { get; private set; }

            [Serializable]
            public class WinLoseStarSettings
            {
                [field: SerializeField] public float percentage { get; private set; } = 0;
            }


            public void Initialize()
            {
                gunRotationSpeed.Initialize();
                musicValue.Initialize();
                soundsVolume.Initialize();
                vibroSettings.Initialize();
                randomPictureSettings.Initialize();

                SetGunRotationSpeed(gunRotationSpeed.currentValue);
                SetMusicVolume(musicValue.currentValue);
                SetSoundsVolume(soundsVolume.currentValue);
                SetRamdonPicturesEnabled(randomPictureSettings.currentValue);
                SetVibroEnabled(vibroSettings.currentValue);
            }

            public void SetGunRotationSpeed(float newGunRotationSpeed)
            {
                gunRotationSpeed.SetData(newGunRotationSpeed);
            }

            public void SetMusicVolume(float newMusicVolume)
            {
                musicValue.SetData(newMusicVolume);

                if (newMusicVolume <= -30) { newMusicVolume = -80; }

                audioMixer.SetFloat("Music", newMusicVolume);
            }

            public void SetSoundsVolume(float newSoundsVolume)
            {
                soundsVolume.SetData(newSoundsVolume);

                if (newSoundsVolume <= -30) { newSoundsVolume = -80; }

                audioMixer.SetFloat("Sounds", newSoundsVolume);
            }

            public void SetVibroEnabled(bool isVibroEnabledNewValue)
            {
                vibroSettings.SetData(isVibroEnabledNewValue);
                VibrationHelper.SetActive(isVibroEnabledNewValue);
            }

            public void SetRamdonPicturesEnabled(bool ramdonPicturesNewValue)
            {
                randomPictureSettings.SetData(ramdonPicturesNewValue);
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
    }
}