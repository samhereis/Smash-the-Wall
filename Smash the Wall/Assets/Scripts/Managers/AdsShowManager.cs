using InGameStrings;
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

        public async void TryShowInterstitial()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.interstitialAd) == false)
            {
                await RequestInterstitial();
            }
        }

        private async void TryShowAppOpen()
        {
            if (await AdsManager.instance.TryShowPlacement(AdsStrings.appOpenAd) == true)
            {
                _appOpenCount = 0;
            }

            await RequestAppOpen();
        }

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

        private async Task RequestInterstitial()
        {
            await AdsManager.instance.Request(AdsStrings.interstitialAd, 5f);
        }

        private async Task RequestBanner()
        {
            await AdsManager.instance.Request(AdsStrings.bannerAd, 5f);
        }

        private async Task RequestAppOpen()
        {
            await AdsManager.instance.Request(AdsStrings.appOpenAd, 5f);
        }
    }
}