using Configs;
using DI;
using InGameStrings;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : CanvasWindowBase
    {
        [Header("DI")]
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;

        [Header("Components")]
        [SerializeField] private Button _closeButton;

        [Space(10)]
        [SerializeField] private Image _buttonsInfoBlock;

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);  

            SubscribeToEvents();

            _buttonsInfoBlock.color = _uIConfigs.uiBackgroundColor_Standart;
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _closeButton.onClick.AddListener(Close);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _closeButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            Disable();
        }
    }
}