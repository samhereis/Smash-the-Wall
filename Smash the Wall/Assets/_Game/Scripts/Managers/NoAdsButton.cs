using DependencyInjection;
using DG.Tweening;
using UnityEngine;
using Services;
using InGameStrings;
using static Services.InAppPurchacesManager;

namespace Managers
{
    public class NoAdsButton : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [Inject][SerializeField] private InAppPurchacesManager _inAppPurchacesManager;
        [Inject][SerializeField] private AdsShowManager _adsShowManager;

        public bool isNoAdsEnabled
        {
            get
            {
                if (_inAppPurchacesManager == null)
                {
                    DependencyInjector.InjectDependencies(this);
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
            /*(this as IDIDependent).LoadDependencies();

            if (_inAppPurchacesManager != null)
            {
                _inAppPurchacesManager.onPurchase += OnAPurchaseCompleted;
                _inAppPurchacesManager.onPurchaseFailed += OnAPurchaseFailed;
                _inAppPurchacesManager.onInitialize += OnInAppPurchacesManagerInitialized;
            }

            UpdateStatus();*/

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
            //_adsShowManager.RemoveAds();

            transform.DOScale(0, 1);
        }

        private void OnNoAdsDisabled()
        {
            transform.DOScale(1, 1);
        }
    }
}