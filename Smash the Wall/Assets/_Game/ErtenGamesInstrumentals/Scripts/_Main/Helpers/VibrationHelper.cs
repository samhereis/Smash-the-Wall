namespace Helpers
{
    public class VibrationHelper
    {
        public bool canVibrate { get; protected set; }

        public virtual void SetActive(bool active)
        {
#if FeelInstalled

            HapticController.hapticsEnabled = active;

#endif
        }

        public virtual void LightVibration()
        {
#if FeelInstalled

            if (Validate() == false) return;

            if (_gameConfigs.gameSettings.vibroSettings.currentValue == false) return;

            Debug.Log("LightImpact");
            try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); } catch { }

#endif
        }

        public virtual void MeduimVibration()
        {
#if FeelInstalled


            if (Validate() == false) return;

            if (_gameConfigs.gameSettings.vibroSettings.currentValue == false) return;

            Debug.Log("MediumImpact");
            try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact); } catch { }

#endif
        }

        public virtual void HardVibration()
        {
#if FeelInstalled

            if (Validate() == false) return;

            if (_gameConfigs.gameSettings.vibroSettings.currentValue == false) return;

            Debug.Log("HeavyImpact");
            try { HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact); } catch { }

#endif
        }

        protected virtual bool Validate()
        {
            bool isSuccesfull = false;

#if FeelInstalled

            if (_gameConfigs == null)
            {
                _gameConfigs = DIBox.Get<GameConfigs>(DIStrings.gameConfigs);
            }

            isSuccesfull = _gameConfigs != null;

#endif

            return isSuccesfull;
        }
    }
}