using DG.Tweening;
using DI;
using InGameStrings;
using UnityEngine;
using static Managers.InAppPurchacesManager;

namespace Managers
{
    public class NoAdsButton : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.inAppPurchacesManager)][SerializeField] private InAppPurchacesManager _inAppPurchacesManager;

        public bool isNoAdsEnabled
        {
            get
            {
                if (_inAppPurchacesManager == null)
                {
                    (this as IDIDependent).LoadDependencies();
                }

                if (_inAppPurchacesManager == null)
                {
                    return false;
                }

                return _inAppPurchacesManager.IsPurchased(InGameStrings.PurchaseStrings.noAdsPurchaseString);
            }
        }

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            if (_inAppPurchacesManager != null)
            {
                _inAppPurchacesManager.onPurchase += OnAPurchaseCompleted;
                _inAppPurchacesManager.onPurchaseFailed += OnAPurchaseFailed;
                _inAppPurchacesManager.onInitialize += OnInAppPurchacesManagerInitialized;
            }

            transform.localScale = Vector3.zero;
            UpdateStatus();
        }

        private void OnDestroy()
        {
            if (_inAppPurchacesManager != null)
            {
                _inAppPurchacesManager.onPurchase -= OnAPurchaseCompleted;
                _inAppPurchacesManager.onPurchaseFailed -= OnAPurchaseFailed;
                _inAppPurchacesManager.onInitialize -= OnInAppPurchacesManagerInitialized;
            }

            transform.DOKill();
        }

        private void OnInAppPurchacesManagerInitialized()
        {
            if (isNoAdsEnabled == false)
            {
                OnNoAdsDisabled();
            }
            else
            {
                OnNoAdsEnabled();
            }
        }

        private void OnAPurchaseCompleted(Item item)
        {
            if (item.id == PurchaseStrings.noAdsPurchaseString)
            {
                OnNoAdsEnabled();
            }
        }

        private void OnAPurchaseFailed(Item item)
        {
            if (item.id == PurchaseStrings.noAdsPurchaseString)
            {
                OnNoAdsDisabled();
            }
        }

        public void TryBuyNoAds()
        {
            _inAppPurchacesManager?.Purchase(_inAppPurchacesManager.GetItem(PurchaseStrings.noAdsPurchaseString));
        }

        private void UpdateStatus()
        {
            if (isNoAdsEnabled == false)
            {
                OnNoAdsDisabled();
            }
            else
            {
                OnNoAdsEnabled();
            }
        }

        private void OnNoAdsEnabled()
        {
            if (AdsManager.instance?.removedAds == false)
            {
                AdsManager.instance?.RemoveAds(true);
            }

            transform.DOScale(0, 1);
        }

        private void OnNoAdsDisabled()
        {
            transform.DOScale(1, 1);
        }
    }
}