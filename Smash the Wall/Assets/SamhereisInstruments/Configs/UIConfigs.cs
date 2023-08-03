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

        [field: SerializeField, Tooltip("Время скейлинга UI элементов")] public float uiScaleAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField, Tooltip("Ease скейлинга UI элементов")] public Ease uiScaleEase { get; private set; } = Ease.InOutBack;
        [field: SerializeField, Tooltip("Время скейлинга UI элементов")] public float uiFadeAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField, Tooltip("Ease скейлинга UI элементов")] public Ease uiFadeEase { get; private set; } = Ease.InOutBack;

        [field: SerializeField, Tooltip("Задний фон внегеймплейных UI окон")] public Color uiBackgroundColor { get; private set; } = Color.cyan;

        public override void Initialize()
        {

        }
    }
}