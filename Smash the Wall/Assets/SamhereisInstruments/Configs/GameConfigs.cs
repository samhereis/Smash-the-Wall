using Helpers;
using Interfaces;
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
            private const string _isVibroEnabledString = "isVibroEnabled";
            private const string _isMusicEnabledString = "isMusicEnabled";
            private const string _areSoundsEnabledString = "areSoundsEnabled";

            private const string _ramdonEnviromentString = "ramdonEnviroment";
            private const string _ramdonPicturesString = "ramdonPictures";

            public static bool isVibroEnabled => PlayerPrefs.GetInt(_isVibroEnabledString, 1) == 1;
            public static bool isMusicEnabled => PlayerPrefs.GetInt(_isMusicEnabledString, 1) == 1;
            public static bool areSoundsEnabled => PlayerPrefs.GetInt(_areSoundsEnabledString, 1) == 1;

            public static bool isRamdonEnviromentEnabled => PlayerPrefs.GetInt(_ramdonEnviromentString, 0) == 1;
            public static bool isRamdonPicturesEnabled => PlayerPrefs.GetInt(_ramdonPicturesString, 0) == 1;

            public Action<bool> onVibroEnabledChanged;
            public Action<bool> onMusicEnabledChanged;
            public Action<bool> onSoundsEnabledChanged;

            public Action<bool> onRamdonEnviromentEnabledChanged;
            public Action<bool> onRamdonPicturesEnabledChanged;

            public Action onSettingsChanged;

#if UNITY_EDITOR

            [Header("Debug")]
            public bool isVibroEnabledDebug;
            public bool isMusicEnabledDebug;
            public bool areSoundsEnabledDebug;

            public bool isRamdonEnviromentEnabledDebug;
            public bool isRamdonPicturesEnabledDebug;

#endif
            public void Initialize()
            {
                UpdateDebug();
            }

            public void SetVibroEnabled(bool isVibroEnabledNewValue)
            {
                PlayerPrefs.SetInt(_isVibroEnabledString, isVibroEnabledNewValue ? 1 : 0);

                VibrationHelper.SetActive(isVibroEnabled);

                OnSettingsChanged();
                onVibroEnabledChanged?.Invoke(isVibroEnabled);
            }

            public void SetMusicEnabled(bool isMusicEnabledNewValue)
            {
                PlayerPrefs.SetInt(_isMusicEnabledString, isMusicEnabledNewValue ? 1 : 0);

                OnSettingsChanged();
                onMusicEnabledChanged?.Invoke(isMusicEnabled);
            }

            public void SetSoundsEnabled(bool areSoundsEnabledNewValue)
            {
                PlayerPrefs.SetInt(_areSoundsEnabledString, areSoundsEnabledNewValue ? 1 : 0);

                OnSettingsChanged();
                onSoundsEnabledChanged?.Invoke(areSoundsEnabled);
            }

            public void SetRamdonEnviromentEnabled(bool ramdonEnviromentNewValue)
            {
                PlayerPrefs.SetInt(_ramdonEnviromentString, ramdonEnviromentNewValue ? 1 : 0);

                OnSettingsChanged();
                onRamdonEnviromentEnabledChanged?.Invoke(isRamdonEnviromentEnabled);
            }

            public void SetRamdonPicturesEnabled(bool ramdonPicturesNewValue)
            {
                PlayerPrefs.SetInt(_ramdonPicturesString, ramdonPicturesNewValue ? 1 : 0);

                OnSettingsChanged();
                onRamdonPicturesEnabledChanged?.Invoke(isRamdonPicturesEnabled);
            }

            private void OnSettingsChanged()
            {
                UpdateDebug();
                onSettingsChanged?.Invoke();
            }

            private void UpdateDebug()
            {

#if UNITY_EDITOR

                isVibroEnabledDebug = isVibroEnabled;
                isMusicEnabledDebug = isMusicEnabled;
                areSoundsEnabledDebug = areSoundsEnabled;

                isRamdonEnviromentEnabledDebug = isRamdonEnviromentEnabled;
                isRamdonPicturesEnabledDebug = isRamdonPicturesEnabled;

#endif
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
            [field: SerializeField] public float gunRotationSpeed { get; private set; } = 0.2f;

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
        }
    }
}