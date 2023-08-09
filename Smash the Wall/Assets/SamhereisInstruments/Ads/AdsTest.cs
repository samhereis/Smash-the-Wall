using DI;
using InGameStrings;
using UnityEngine;

namespace Managers
{
    public class AdsTest : MonoBehaviour, IDIDependent
    {
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        [ContextMenu(nameof(ShowBanner))]
        public void ShowBanner()
        {
            Debug.Log(nameof(ShowBanner));
            _adsShowManager.ShowBanner();
        }

        [ContextMenu(nameof(ShowInterstitial))]
        public void ShowInterstitial()
        {
            Debug.Log(nameof(ShowBanner));
            _adsShowManager.TryShowInterstitial();
        }

        [ContextMenu(nameof(ShowRewarded))]
        public void ShowRewarded()
        {
            Debug.Log(nameof(ShowRewarded));
            _adsShowManager.TryShowRewarded();
        }

        [ContextMenu(nameof(ShowAppOpen))]
        public void ShowAppOpen()
        {
            Debug.Log(nameof(ShowAppOpen));
            _adsShowManager.TryShowAppOpen();
        }
    }
}