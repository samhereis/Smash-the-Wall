using InGameStrings;
using System;
using System.Threading.Tasks;
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
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private async void Start()
        {
            await RequestInterstitial();
            await RequestRewarded();
            await RequestBanner();
            await RequestAppOpen();
        }

        private void OnApplicationFocus(bool focus)
        {
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

        #region Rewarded

        public async void TryShowRewarded(Action callback)
        {
            AdsManager.instance.OnRewarded -= OnRewarded;
            AdsManager.instance.OnClose -= OnAdClosed;

            AdsManager.instance.OnRewarded += OnRewarded;
            AdsManager.instance.OnClose += OnAdClosed;

            if (await AdsManager.instance.TryShowPlacement(AdsStrings.rewardedAd) == false)
            {
                await RequestRewarded();

                await AdsManager.instance.TryShowPlacement(AdsStrings.rewardedAd);
            }

            void OnRewarded(Placement placement)
            {
                AdsManager.instance.OnRewarded -= OnRewarded;
                callback?.Invoke();
            }

            void OnAdClosed(Placement placement)
            {
                AdsManager.instance.OnClose -= OnAdClosed;
            }
        }

        private async Task RequestRewarded()
        {
            await AdsManager.instance.Request(AdsStrings.rewardedAd, 5f);
        }

        #endregion

        #region Interstitial

        public async void TryShowInterstitial()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.interstitialAd) == false)
            {
                await RequestInterstitial();
            }
        }

        private async Task RequestInterstitial()
        {
            await AdsManager.instance.Request(AdsStrings.interstitialAd, 5f);
        }

        #endregion

        #region Banner

        public async void ShowBanner()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.bannerAd) == false)
            {
                await RequestBanner();
                await AdsManager.instance.TryShowPlacement(AdsStrings.bannerAd);
            }
        }

        public void DestroyBanner()
        {
            AdsManager.instance.Destroy(AdsStrings.bannerAd);
        }

        private async Task RequestBanner()
        {
            await AdsManager.instance.Request(AdsStrings.bannerAd, 5f);
        }

        #endregion

        #region AppOpen

        private async void TryShowAppOpen()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.appOpenAd) == true)
            {
                _appOpenCount = 0;
            }

            await RequestAppOpen();
        }

        private async Task RequestAppOpen()
        {
            await AdsManager.instance.Request(AdsStrings.appOpenAd, 5f);
        }

        #endregion
    }
}