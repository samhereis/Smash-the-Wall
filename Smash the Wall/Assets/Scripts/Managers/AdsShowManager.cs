using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using InGameStrings;
using UnityEngine;

namespace Managers
{
    public class AdsShowManager : MonoBehaviour
    {
        public static AdsShowManager instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int _openAppCountToShowAppOpenAdd = 1;

        [Header("Debug")]
        [SerializeField] private int _appOpenCount = 0;

        private void Awake()
        {
            instance = this;
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        private void OnDestroy()
        {
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
            instance = null;
        }

        private async void OnAppStateChanged(AppState state)
        {
            Debug.Log("App State changed to : " + state);

            bool isForeground = state == AppState.Foreground;
            bool isAppOpenCountReached = _appOpenCount >= _openAppCountToShowAppOpenAdd;

            if (isForeground)
            {
                _appOpenCount++;

                if (isAppOpenCountReached)
                {
                    await AdsManager.instance.TryShowPlacement(AdsStrings.appOpenAd);
                    RequestAppOpen();
                }
            }
        }

        public async void TryShowInterstitial()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.interstitialAd) == false)
            {
                Debug.LogWarning("Interstitial ads not ready");
            }
            else
            {
                Debug.Log("Interstitial ads shown");
            }

            RequestInterstitial();
        }

        public async void ShowBanner()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.bannerAd) == false)
            {
                Debug.LogWarning("Banner ads not ready");
            }
            else
            {
                Debug.Log("Banner ads shown");
            }

            RequestBanner();
        }

        private async void RequestInterstitial()
        {
            await AdsManager.instance.Request(AdsStrings.interstitialAd, 5f);
        }

        private async void RequestBanner()
        {
            await AdsManager.instance.Request(AdsStrings.bannerAd, 5f);
        }

        private async void RequestAppOpen()
        {
            await AdsManager.instance.Request(AdsStrings.appOpenAd, 5f);
        }
    }
}