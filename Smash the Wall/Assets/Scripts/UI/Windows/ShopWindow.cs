using Configs;
using DI;
using InGameStrings;
using ProjectSripts;
using UI.Canvases;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopWindow : CanvasWindowBase
    {
        [Header("Components")]
        [SerializeField] private Button _backButton; 

        [Header("Shops")]
        [SerializeField] private WeaponsShop _weaponsShop;

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _weaponsShop.Initialize();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();

            _weaponsShop.Clear();
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