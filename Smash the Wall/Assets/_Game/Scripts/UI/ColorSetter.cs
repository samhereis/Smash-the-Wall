using AYellowpaper.SerializedCollections;
using Configs;
using DI;
using Helpers;
using InGameStrings;
using SO;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class ColorSetter : MonoBehaviour, IDIDependent
    {
        private static UIConfigs _uIConfigs;

        [Header("Components")]
        [SerializeField] private SerializedDictionary<ColorSetUnitString, Graphic[]> _colorSetUnits = new SerializedDictionary<ColorSetUnitString, Graphic[]>();

        private async void OnEnable()
        {
            while (BindDIScene.isGLoballyInjected == false) { await AsyncHelper.Delay(20); }

            if (_uIConfigs == null)
            {
                _uIConfigs = DIBox.Get<UIConfigs>(DIStrings.uiConfigs);
            }

            SetColors();
        }

        private async void SetColors()
        {
            try
            {
                foreach (var colorSetUnit in _colorSetUnits)
                {
                    await AsyncHelper.Delay();

                    foreach (var graphic in colorSetUnit.Value)
                    {
                        graphic.color = _uIConfigs.colorSetUnits[colorSetUnit.Key];
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error applying colors: " + ex);
            }
        }
    }
}