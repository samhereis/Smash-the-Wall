using InGameStrings;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AdsManager))]
    public class AdsShowManager : MonoBehaviour
    {
        [Header("Comonents")]
        [SerializeField] private AdsManager _adsManager;

        [Header("Settings")]
        [SerializeField] private int _openAppCountToShowAppOpenAdd = 1;

        [Header("Debug")]
        [SerializeField] private int _appOpenCount = 0;

        private void Awake()
        {
            if (_adsManager == null)
            {
                _adsManager = GetComponent<AdsManager>();
            }
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

#if UNITY_EDITOR == false

            if (focus == true)
            {
                bool isAppOpenCountReached = _appOpenCount >= _openAppCountToShowAppOpenAdd;

                _appOpenCount++;

                if (isAppOpenCountReached)
                {
                    TryShowAppOpen();
                }
            }

#endif

        }

        public void RemoveAds()
        {
            if (_adsManager.removedAds == false)
            {
                _adsManager.RemoveAds(true);
            }
        }

        #region Rewarded

        public async void TryShowRewarded(Action callback = null)
        {
            Debug.Log("Try Show ad: Rewarded");

            _adsManager.OnRewarded -= OnRewarded;
            _adsManager.OnClose -= OnAdClosed;

            _adsManager.OnRewarded += OnRewarded;
            _adsManager.OnClose += OnAdClosed;

            if (await _adsManager.TryShowPlacement(AdsStrings.rewardedAd) == false)
            {
                await RequestRewarded();

                await _adsManager.TryShowPlacement(AdsStrings.rewardedAd);
            }

            void OnRewarded(Placement placement)
            {
                _adsManager.OnRewarded -= OnRewarded;
                callback?.Invoke();
            }

            void OnAdClosed(Placement placement)
            {
                _adsManager.OnClose -= OnAdClosed;
            }
        }

        private async Task RequestRewarded()
        {
            await _adsManager.Request(AdsStrings.rewardedAd, 5f);
        }

        #endregion

        #region Interstitial

        public async void TryShowInterstitial()
        {
            Debug.Log("Try Show ad: Interstitial");

            if (await _adsManager.TryShowPlacement(AdsStrings.interstitialAd) == false)
            {
                await RequestInterstitial();
            }
        }

        private async Task RequestInterstitial()
        {
            await _adsManager.Request(AdsStrings.interstitialAd, 5f);
        }

        #endregion

        #region Banner

        public async void TryShowBanner()
        {
            Debug.Log("Try Show ad: Banner");

            if (await _adsManager.TryShowPlacement(AdsStrings.bannerAd) == false)
            {
                await RequestBanner();
                await _adsManager.TryShowPlacement(AdsStrings.bannerAd);
            }
        }

        public void DestroyBanner()
        {
            _adsManager.Destroy(AdsStrings.bannerAd);
        }

        private async Task RequestBanner()
        {
            await _adsManager.Request(AdsStrings.bannerAd, 5f);
        }

        #endregion

        #region AppOpen

        public async void TryShowAppOpen()
        {
            Debug.Log("Try Show ad: AppOpen");

            if (await _adsManager.TryShowPlacement(AdsStrings.appOpenAd) == true)
            {
                _appOpenCount = 0;
            }

            await RequestAppOpen();
        }

        private async Task RequestAppOpen()
        {
            await _adsManager.Request(AdsStrings.appOpenAd, 5f);
        }

        #endregion
    }
}