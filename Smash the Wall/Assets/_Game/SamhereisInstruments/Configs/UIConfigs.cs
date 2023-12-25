using DG.Tweening;
using Helpers;
using SO;
using System;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "UIConfigs", menuName = "Scriptables/Config/UIConfigs")]
    public class UIConfigs : ConfigBase
    {
        public static float defaultUIScaleAnimationDuration { get; private set; } = 0.25f;
        public static Ease defaultUIScaleEase { get; private set; } = Ease.OutBack;
        public static float defaultUIFadeAnimationDuration { get; private set; } = 0.25f;
        public static Ease defaultUIFadeEase { get; private set; } = Ease.OutBack;

        [field: SerializeField, Space(10), Header("Animations")] public float uiScaleAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease uiScaleEase { get; private set; } = Ease.InOutBack;
        [field: SerializeField] public float uiFadeAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease uiFadeEase { get; private set; } = Ease.InOutBack;

        [field: SerializeField, Space(10)] public float uiAnimationElementForeachDelay { get; private set; } = 0.025f;

        public override void Initialize()
        {

        }

        public void SetUIAnimationElementForeachDelay(float newUIAnimationElementForeachDelay)
        {
            uiAnimationElementForeachDelay = newUIAnimationElementForeachDelay;
        }
    }
}