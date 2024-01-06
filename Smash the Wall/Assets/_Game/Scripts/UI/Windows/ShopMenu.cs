using Sirenix.OdinInspector;
using UI.Canvases;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopMenu : MenuBase
    {
        [Header("Components")]

        [Required]
        [SerializeField] private Button _backButton;

        [Header("Shops")]

        [Required]
        [SerializeField] private WeaponsShop _weaponsShop;

        private MenuBase _openOnClose;

        public void Initialize(MenuBase openOnClose)
        {
            _openOnClose = openOnClose;
        }

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

            _weaponsShop.Dispose();

            _openOnClose?.Enable();
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