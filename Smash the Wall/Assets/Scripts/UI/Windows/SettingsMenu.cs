using DI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : CanvasWindowBase, IDIDependent
    {
        [Header("Components")]
        [SerializeField] private Button _closeButton;

        protected void Start()
        {
            (this as IDIDependent).LoadDependencies();

            Disable(0);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);  

            SubscribeToEvents();
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