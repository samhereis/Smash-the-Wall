using DependencyInjection;
using ErtenGamesInstrumentals.Ads;
using System;
using UnityEngine;

namespace Services
{
    public class AdsShowManager : MonoBehaviour
    {
        public class AdsStrings
        {
            public const string DefaultInterstitial = nameof(DefaultInterstitial);
            public const string DefaultAppOpen = nameof(DefaultAppOpen);
            public const string DefaultRewarded = nameof(DefaultRewarded);
            public const string DefaultBanner = nameof(DefaultBanner);
        }

        [Header("Settings")]
        [SerializeField] private int _openAppCountToShowAppOpenAdd = 1;
        [SerializeField] private bool _allowAppOpenInEditor = false;
        [SerializeField] private bool _isActive = false;

        [Header("Debug")]
        [SerializeField] private int _appOpenCount = 0;

        [Inject] private IRewardedAd _rewardedAd;
        [Inject] private IInterstitialAd _interstitialAd;
        [Inject] private IBannerAd _bannerAd;
        [Inject] private IAppOpenAd _appOpenAd;

        private void Start()
        {
            RequestInterstitial();
            RequestRewarded();
            RequestBanner();
            RequestAppOpen();
        }

        private void OnApplicationFocus(bool focus)
        {
#if UNITY_EDITOR
            if (_allowAppOpenInEditor == false) { return; }
#endif
            if (focus == true)
            {
                bool isAppOpenCountReached = _appOpenCount >= _openAppCountToShowAppOpenAdd;

                _appOpenCount++;

                if (isAppOpenCountReached)
                {
                    TryShowAppOpen();
                }
            }
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        #region Rewarded

        public void TryShowRewarded(string appID = nameof(AdsStrings.DefaultRewarded), Action onRewarded = null, Action onFail = null)
        {
            Debug.Log("Try Show ad: Rewarded");

            _rewardedAd?.TryShowRewardedAd(appID, onRewarded, onFail);
        }

        private void RequestRewarded(string adID = nameof(AdsStrings.DefaultRewarded))
        {
            _rewardedAd.Request();
        }

        #endregion

        #region Interstitial

        public void TryShowInterstitial(string adID = nameof(AdsStrings.DefaultInterstitial))
        {
            Debug.Log("Try Show ad: Interstitial");

            _interstitialAd?.TryShowInterstitial(adID);
        }

        private void RequestInterstitial(string adID = nameof(AdsStrings.DefaultRewarded))
        {
            _interstitialAd?.Request();
        }

        #endregion

        #region Banner

        public void TryShowBanner(string adID = nameof(AdsStrings.DefaultBanner))
        {
            Debug.Log("Try Show ad: Banner");

            _bannerAd?.TryShowBanner(adID);
        }

        public void DestroyBanner(string adID = nameof(AdsStrings.DefaultBanner))
        {
            _bannerAd?.Destroy();
        }

        private void RequestBanner(string adID = nameof(AdsStrings.DefaultBanner))
        {
            _bannerAd?.Request();
        }

        #endregion

        #region AppOpen

        public void TryShowAppOpen(string adID = nameof(AdsStrings.DefaultAppOpen))
        {
            Debug.Log("Try Show ad: AppOpen");

            _appOpenAd.TryShowAppOpenAd(adID, onShowed: () =>
            {
                _appOpenCount = 0;
            },
            onFail: () =>
            {

            });
        }

        private void RequestAppOpen(string adID = nameof(AdsStrings.DefaultAppOpen))
        {
            _appOpenAd?.Request();
        }

        #endregion
    }
}