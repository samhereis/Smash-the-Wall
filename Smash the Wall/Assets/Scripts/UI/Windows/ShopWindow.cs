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
    public class ShopWindow : CanvasWindowBase, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;

        [Header("Components")]
        [SerializeField] private Button _backButton; 
        
        [Space(10)]
        [SerializeField] private Image _upperPartBackground;
        [SerializeField] private Image _mainPartBackground;

        [Header("Shops")]
        [SerializeField] private WeaponsShop _weaponsShop;

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

            _weaponsShop.Initialize();

            _upperPartBackground.color = _uIConfigs.uiBackgroundColor_Shop_UpperPart;
            _mainPartBackground.color = _uIConfigs.uiBackgroundColor_Shop_MainPart;
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