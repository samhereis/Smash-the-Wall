using DG.Tweening;
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

        [field: SerializeField, Space(10)] public float uiScaleAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease uiScaleEase { get; private set; } = Ease.InOutBack;
        [field: SerializeField] public float uiFadeAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease uiFadeEase { get; private set; } = Ease.InOutBack;

        [field: SerializeField, Space(10)] public Color uiBackgroundColor_Standart { get; private set; } = Color.cyan;
        [field: SerializeField] public Color uiBackgroundColor_Win { get; private set; } = Color.red;
        [field: SerializeField] public Color uiBackgroundColor_Lose { get; private set; } = Color.green;
        [field: SerializeField] public Color uiBackgroundColor_Shop_MainPart { get; private set; } = Color.yellow;
        [field: SerializeField] public Color uiBackgroundColor_Shop_UpperPart { get; private set; } = Color.cyan;


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