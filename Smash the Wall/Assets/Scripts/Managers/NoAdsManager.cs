using Backend;
using DI;
using Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Managers.InAppPurchacesManager;

namespace Managers
{
    public class NoAdsManager : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [DI(InGameStrings.inGameStringsString)][SerializeField] private InGameStrings _inGameStrings;
        [DI(InGameStrings.DIStrings.inAppPurchacesManager)][SerializeField] private InAppPurchacesManager _inAppPurchacesManager;

        private List<Button> _noAdsButtons = new List<Button>();

        public bool isNoAdsEnabled
        {
            get
            {
                return true;

                /*if(_inAppPurchacesManager == null)
                {
                    (this as IDIDependent).LoadDependencies();
                }

                return _inAppPurchacesManager.IsPurchased(InGameStrings.PurchaseStrings.noAdsPurchaseString);*/
            }
        }

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            if (_inAppPurchacesManager == null) return;

            _inAppPurchacesManager.onPurchaseCompleted += OnAPurchaseCompleted;
            _inAppPurchacesManager.onPurchaseFailed += OnAPurchaseFailed;
            _inAppPurchacesManager.OnInitialize += OnInAppPurchacesManagerInitialized;
        }

        private void OnDestroy()
        {
            if (_inAppPurchacesManager == null) return;

            _inAppPurchacesManager.onPurchaseCompleted -= OnAPurchaseCompleted;
            _inAppPurchacesManager.onPurchaseFailed -= OnAPurchaseFailed;
            _inAppPurchacesManager.OnInitialize -= OnInAppPurchacesManagerInitialized;
        }

        public void AddNoAdsButton(Button noAdsButton)
        {
            _noAdsButtons.SafeAdd(noAdsButton);

            if (isNoAdsEnabled == false)
            {
                DisableAllButtons();
                EnableAllButtons();

                noAdsButton.UnregisterCallback<ClickEvent>(TryBuyNoAds);
                noAdsButton.RegisterCallback<ClickEvent>(TryBuyNoAds);
            }
            else
            {
                DisableAllButtons();
                Debug.Log("No ads was enabled");
            }
        }

        private void OnInAppPurchacesManagerInitialized()
        {
            if (isNoAdsEnabled == false)
            {
                EnableAllButtons();
            }
            else
            {
                DisableAllButtons();
            }
        }

        private void OnAPurchaseCompleted(Item item)
        {
            if (item.id == InGameStrings.PurchaseStrings.noAdsPurchaseString)
            {
                DisableAllButtons();
            }
        }

        private void OnAPurchaseFailed(Item item)
        {
            if (item.id == InGameStrings.PurchaseStrings.noAdsPurchaseString)
            {
                EnableAllButtons();
            }
        }

        public void TryBuyNoAds(ClickEvent evt)
        {
            DisableAllButtons();
            _inAppPurchacesManager.Purchase(InGameStrings.PurchaseStrings.noAdsPurchaseString);
        }

        private void EnableAllButtons()
        {
            foreach (var button in _noAdsButtons)
            {
                button.SetEnabled(true);
                button.visible = true;
            }
        }

        private void DisableAllButtons()
        {
            foreach (var button in _noAdsButtons)
            {
                button.SetEnabled(false);
                button.visible = false;
            }
        }
    }
}