using Configs;
using UnityEngine;

namespace Helpers
{
    public static class VibrationHelper
    {
        public static void SetActive(bool active)
        {
            //HapticController.hapticsEnabled = active;
        }

        public static void LightImpact()
        {
            if (GameConfigs.GameSettings.isVibroEnabled == false) return;

            Debug.Log("MediumImpact");
            //try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact); } catch { }
        }

        public static void MeduimVibration()
        {
            if (GameConfigs.GameSettings.isVibroEnabled == false) return;

            Debug.Log("HeavyImpact");
            //try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact); } catch { }
        }
    }
}