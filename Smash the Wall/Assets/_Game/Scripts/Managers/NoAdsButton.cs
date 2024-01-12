#if InAppPurchacesInstalled

using DependencyInjection;
using DG.Tweening;
using InGameStrings;
using Services;
using UnityEngine;

using static Services.InAppPurchacesManager;

namespace Managers
{
    public class NoAdsButton : MonoBehaviour, INeedDependencyInjection
    {
        [Header("DI")]
#if InAppPurchacesInstalled
        [Inject][SerializeField] private InAppPurchacesManager _inAppPurchacesManager;
#endif

        [Inject][SerializeField] private AdsShowManager _adsShowManager;

        public bool isNoAdsEnabled
        {
            get
            {
                if (_inAppPurchacesManager == null)
                {
                    DependencyContext.InjectDependencies(this);
                }

                if (_inAppPurchacesManager == null)
                {
                    return false;
                }

                return _inAppPurchacesManager.IsPurchased(InGameStrings.PurchaseStrings.NoAdsPurchaseString);
            }
        }

        private void Start()
        {
            DependencyContext.diBox.InjectDataTo(this);

            if (_inAppPurchacesManager != null)
            {
                _inAppPurchacesManager.onPurchase += OnAPurchaseCompleted;
                _inAppPurchacesManager.onPurchaseFailed += OnAPurchaseFailed;
                _inAppPurchacesManager.onInitialize += OnInAppPurchacesManagerInitialized;
            }

            UpdateStatus();

            transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
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
            if (isNoAdsEnabled == true)
            {
                OnNoAdsEnabled();
            }
            else
            {
                OnNoAdsDisabled();
            }
        }

        private void OnAPurchaseCompleted(Item item)
        {
            if (item.id == PurchaseStrings.NoAdsPurchaseString)
            {
                OnNoAdsEnabled();
            }
        }

        private void OnAPurchaseFailed(Item item)
        {
            if (item.id == PurchaseStrings.NoAdsPurchaseString)
            {
                OnNoAdsDisabled();
            }
        }

        public void TryBuyNoAds()
        {
            _inAppPurchacesManager?.Purchase(_inAppPurchacesManager.GetItem(PurchaseStrings.NoAdsPurchaseString));
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
            _adsShowManager?.RemoveAds();

            transform.DOScale(0, 1);
        }

        private void OnNoAdsDisabled()
        {
            transform.DOScale(1, 1);
        }
    }
}

#endif