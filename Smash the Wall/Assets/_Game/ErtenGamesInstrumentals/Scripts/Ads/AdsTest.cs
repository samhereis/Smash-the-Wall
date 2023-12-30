using Sirenix.OdinInspector;
using UnityEngine;

namespace Services
{
    public class AdsTest : MonoBehaviour
    {
        [Required]
        [SerializeField] private AdsShowManager _adsShowManager;

        [Button]
        public void ShowBanner()
        {
            Debug.Log(nameof(ShowBanner));
            _adsShowManager.TryShowBanner();
        }

        [Button]
        public void ShowInterstitial()
        {
            Debug.Log(nameof(ShowBanner));
            _adsShowManager.TryShowInterstitial();
        }

        [Button]
        public void ShowRewarded()
        {
            Debug.Log(nameof(ShowRewarded));
            _adsShowManager.TryShowRewarded();
        }

        [Button]
        public void ShowAppOpen()
        {
            Debug.Log(nameof(ShowAppOpen));
            _adsShowManager.TryShowAppOpen();
        }
    }
}