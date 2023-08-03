using UnityEngine;

namespace Managers
{
    public class AdsTest : MonoBehaviour
    {
        public string bannerString = "banner";
        public string interstitialString = "interstitial";
        public string rewardedString = "rewarded";
        public string appOpenString = "appOpen";

        [ContextMenu(nameof(RequestBanner))]
        public async void RequestBanner()
        {
            Debug.Log($"{nameof(RequestBanner)} {await AdsManager.instance.Request(bannerString, 5)}");
        }

        [ContextMenu(nameof(ShowBanner))]
        public async void ShowBanner()
        {
            Debug.Log($"{nameof(ShowBanner)} {await AdsManager.instance.TryShowPlacement(bannerString)}");
        }

        [ContextMenu(nameof(RequestInterstitial))]
        public async void RequestInterstitial()
        {
            Debug.Log($"{nameof(RequestInterstitial)} {await AdsManager.instance.Request(interstitialString, 5)}");
        }

        [ContextMenu(nameof(ShowInterstitial))]
        public async void ShowInterstitial()
        {
            Debug.Log($"{nameof(ShowInterstitial)} {await AdsManager.instance.TryShowPlacement(interstitialString)}");
        }

        [ContextMenu(nameof(RequestRewarded))]
        public async void RequestRewarded()
        {
            Debug.Log($"{nameof(RequestRewarded)} {await AdsManager.instance.Request(rewardedString, 5)}");
        }

        [ContextMenu(nameof(ShowRewarded))]
        public async void ShowRewarded()
        {
            Debug.Log($"{nameof(ShowRewarded)} {await AdsManager.instance.TryShowPlacement(rewardedString)}");
        }

        [ContextMenu(nameof(RequestAppOpen))]
        public async void RequestAppOpen()
        {
            Debug.Log($"{nameof(RequestAppOpen)} {await AdsManager.instance.Request(appOpenString, 5)}");
        }

        [ContextMenu(nameof(ShowAppOpen))]
        public async void ShowAppOpen()
        {
            Debug.Log($"{nameof(ShowAppOpen)} {await AdsManager.instance.TryShowPlacement(appOpenString)}");
        }
    }
}