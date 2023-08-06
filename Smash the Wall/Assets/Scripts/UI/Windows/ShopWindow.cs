using DI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopWindow : CanvasWindowBase, IDIDependent
    {
        [Header("Components")]
        [SerializeField] private Button _backButton;

        protected void Start()
        {
            (this as IDIDependent).LoadDependencies();

            Disable(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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

            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            Disable();
        }
    }
}