using Configs;
using DI;
using InGameStrings;
using UnityEngine;

namespace Helpers
{
    public static class VibrationHelper
    {
        private static GameConfigs _gameConfigs;

        public static void SetActive(bool active)
        {
            //HapticController.hapticsEnabled = active;
        }

        public static void LightImpact()
        {
            if (Validate() == false) return;

            if (_gameConfigs.gameSettings.vibroSettings.currentValue == false) return;

            Debug.Log("MediumImpact");
            //try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact); } catch { }
        }

        public static void MeduimVibration()
        {
            if (Validate() == false) return;

            if (_gameConfigs.gameSettings.vibroSettings.currentValue == false) return;

            Debug.Log("HeavyImpact");
            //try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact); } catch { }
        }

        private static bool Validate()
        {
            if (_gameConfigs == null)
            {
                _gameConfigs = DIBox.Get<GameConfigs>(DIStrings.gameConfigs);
            }

            return _gameConfigs != null;
        }
    }
}