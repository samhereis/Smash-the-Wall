using Configs;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "UIConfigs", menuName = "Scriptables/Config/UIConfigs")]
    public class UIConfigs : ConfigBase
    {
        public const string orderText_SpritePlaceholderString = "orderSpriteHere";

        public static float defaultUIScaleAnimationDuration { get; private set; } = 0.25f;
        public static Ease defaultUIScaleEase { get; private set; } = Ease.OutBack;
        public static float defaultUIFadeAnimationDuration { get; private set; } = 0.25f;
        public static Ease defaultUIFadeEase { get; private set; } = Ease.OutBack;

        [field: SerializeField, Tooltip("Время скейлинга UI элементов")] public float uiScaleAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField, Tooltip("Ease скейлинга UI элементов")] public Ease uiScaleEase { get; private set; } = Ease.InOutBack;
        [field: SerializeField, Tooltip("Время скейлинга UI элементов")] public float uiFadeAnimationDuration { get; private set; } = 0.25f;
        [field: SerializeField, Tooltip("Ease скейлинга UI элементов")] public Ease uiFadeEase { get; private set; } = Ease.InOutBack;

        [field: SerializeField, Tooltip("Дистанция стикера от силуэта для успеха")] public float sctickerDropDistance { get; private set; } = 10;

        [field: SerializeField, Tooltip("Текста заказов (orderSpriteHere - место спрайта заказа)")] public List<string> orderTexts { get; private set; } = new List<string>();
        [field: SerializeField, Tooltip("Задний фон внегеймплейных UI окон")] public Color uiBackgroundColor { get; private set; } = Color.cyan;

        public override void Initialize()
        {

        }
    }
}