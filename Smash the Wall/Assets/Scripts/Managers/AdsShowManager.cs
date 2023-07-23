using InGameStrings;
using UnityEngine;

namespace Managers
{
    public class AdsShowManager : MonoBehaviour
    {
        public static AdsShowManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private async void OnApplicationPause(bool paused)
        {
            if (paused == false)
            {
                await AdsManager.instance.TryShowPlacement(AdsStrings.appOpenAd);
                RequestAppOpen();
            }
        }

        private async void OnApplicationFocus(bool focus)
        {
            if (focus == true)
            {
                await AdsManager.instance.TryShowPlacement(AdsStrings.appOpenAd);
                RequestAppOpen();
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