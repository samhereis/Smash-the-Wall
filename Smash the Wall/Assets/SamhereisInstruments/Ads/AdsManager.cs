using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Managers.Providers;
using System.Threading.Tasks;
using Helpers;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Managers
{
    public enum AdType
    {
        AppOpen,
        Banner,
        Interstitial,
        Rewarded,
        RewardedInterstitial,
        MRec
    }

    public enum Provider
    {
        None,
        AdMob,
        Ironsource,
        CleverAds,
        ApplovinMAX
    }

    /// <summary>
    /// Implemented providers AdMob, IronSource, Clever Ads, Applovin MAX
    /// </summary>
    public class AdsManager : MonoBehaviour
    {
        public static readonly string version = "1.0.4";

        public Action OnGetAdvertissingId;
        public Action OnInitialized;
        public Action<AdType, string> OnError;
        public Action<AdType> OnLoaded;
        public Action<Placement> OnOpen;
        public Action<Placement> OnClose;
        public Action<Placement> OnRewarded;
        public Action<Placement> OnRewardedFailed;
        public Action<Placement, Revenue> OnPaid;
        public Action<string> onGetNullPlacement;
        public Action<AdType> onGetNullOnCurrentPlacement;
        [field: SerializeField] public Provider provider { get; private set; }
        [field: SerializeField] public AdProvider adProvider { get; private set; }

#if ADMOB
        [SerializeField]
        private bool
            _ironSource,
            _adColony,
            _maxSdk,
            _tapjoy,
            _vungle,
            _unityAds,
            _myTarget,
            _dtExchange,
            _chartboost;
#endif


        [Header("ID's")]
#if ADMOB
        [Header("Set app ID's in GoogleMobileAdsSettings")]
        [SerializeField] private Settings _admob_Android;
        [SerializeField] private Settings _admob_Ios;
#endif

#if APPLOVINMAX
        [SerializeField] private Settings applovin;
#endif

#if IRONSOURCE
        [SerializeField] private Settings ironsource;
#endif

#if CLEVERADS
        [SerializeField] private Settings applovin;
#endif
        [Header("Settings")]
        [SerializeField] private bool _dontDestroyOnLoad = true;
        [SerializeField] private bool _initializeOnStart = true;
        [SerializeField] private bool _initializeOnSetConsent = true;
        [SerializeField] private int _defaultAppOpenAdDelay = 5;
        [SerializeField] private int _defaultInterstitialDelay = 45;
        [SerializeField] private int _defaultRewardedDelay = 0;
        [SerializeField] private int _defaultInterstitialAfterRewardedDelay = 45;
        [SerializeField] private bool _isTest;

        [Header("Settings - Ads")]
        [SerializeField] private List<AdType> _enabledAd = new List<AdType>();
        [SerializeField] private List<AdType> _skipAd = new List<AdType>();
        [field: SerializeField] public Placement[] placements { get; private set; }

        [Header("Debug")]
        [SerializeField] private bool _isAdTrackingConsentValueSet;
        [SerializeField] private bool _adTrackingConsent;
        [field: SerializeField] public bool isInitialized { get; private set; }
        [SerializeField] private float _currentAppOpenDelay;

        [SerializeField] private bool _rewardEarned;
        [field: SerializeField] public bool removedAds { get; private set; }
        [field: SerializeField] public string advertisingId { get; private set; }
        [SerializeField] Placement _currentRequestedForShowPlacement;
        [SerializeField] bool _showAdResultChanged;
        [SerializeField] bool _rewardedStatusChanged;
        [SerializeField] bool _lastAdShownSuccessfully;

        Dictionary<AdType, Placement> _currentPlacements = new Dictionary<AdType, Placement>();
        List<string> _defineSymbols = new List<string>();

        public DateTime LastAppOpenAdWatch { get; private set; }
        public DateTime LastInterstitialShow { get; private set; }
        public DateTime LastRewardedShow { get; private set; }

        public bool IsAppOpenDelayed => (DateTime.Now - LastAppOpenAdWatch).TotalSeconds < _defaultAppOpenAdDelay;

        private CancellationTokenSource _getRewardCancellationTokenSource = new CancellationTokenSource();

        private void Awake()
        {
            if (_initializeOnStart && isInitialized == false)
            {
                Initialize();
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (adProvider != null) adProvider.OnPause(paused);
        }

        private void Update()
        {
            if (_currentAppOpenDelay > 0) _currentAppOpenDelay -= Time.deltaTime;
        }

        private void SetDefineSymbols(string key, bool add)
        {
            if (add)
            {
                if (_defineSymbols.Contains(key) == false) _defineSymbols.Add(key);
            }
            else
            {
                _defineSymbols.Remove(key);
            }
        }

        public void ApplySdks()
        {
            SetDefineSymbols("ADMOB", provider == Provider.AdMob);
            SetDefineSymbols("IRONSOURCE", provider == Provider.Ironsource);
            SetDefineSymbols("CLEVERADS", provider == Provider.CleverAds);
            SetDefineSymbols("APPLOVINMAX", provider == Provider.ApplovinMAX);

#if ADMOB
            SetDefineSymbols("ADMOB_IRONSOURCE", _ironSource);
            SetDefineSymbols("ADMOB_ADCOLONY", _adColony);
            SetDefineSymbols("ADMOB_APPLOVIN", _maxSdk);
            SetDefineSymbols("ADMOB_TAPJOY", _tapjoy);
            SetDefineSymbols("ADMOB_VUNGLE", _vungle);
            SetDefineSymbols("ADMOB_UNITYADS", _unityAds);
            SetDefineSymbols("ADMOB_MYRAGET", _myTarget);
            SetDefineSymbols("ADMOB_DTEXCHANGE", _dtExchange);
            SetDefineSymbols("ADMOB_CHARTBOOST", _chartboost);
#endif

#if !ADMOB
            SetDefineSymbols("ADMOB_IRONSOURCE", false);
            SetDefineSymbols("ADMOB_ADCOLONY", false);
            SetDefineSymbols("ADMOB_APPLOVIN", false);
            SetDefineSymbols("ADMOB_TAPJOY", false);
            SetDefineSymbols("ADMOB_VUNGLE", false);
            SetDefineSymbols("ADMOB_UNITYADS", false);
            SetDefineSymbols("ADMOB_MYRAGET", false);
            SetDefineSymbols("ADMOB_DTEXCHANGE", false);
            SetDefineSymbols("ADMOB_CHARTBOOST", false);
#endif

#if UNITY_EDITOR
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(_defineSymbols.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));

            //UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
#endif
        }

        public Placement GetCurrentPlacement(AdType type)
        {
            if (_currentPlacements[type] == null) onGetNullOnCurrentPlacement?.Invoke(type);
            return _currentPlacements[type];
        }

        public Placement GetPlacement(string placement)
        {
            Placement value = null;

            value = placements.FirstOrDefault(x => x.placement.Equals(placement));

            if (value == null)
            {
                Debug.LogWarning($"Placement '{placement}' not found!");
                onGetNullPlacement?.Invoke(placement);
            }

            return value;
        }

        public void SkipAd(params AdType[] types)
        {
            if (types == null) _skipAd = new List<AdType>();
            else _skipAd = types.ToList();
        }

        public void EnableAd(AdType adType, bool enable)
        {
            if (enable)
            {
                if (_enabledAd.Contains(adType) == false) _enabledAd.Add(adType);
            }
            else
            {
                _enabledAd.Remove(adType);
            }
        }

        public bool IsReady(string placement)
        {
            var p = GetPlacement(placement);

            return p != null ? IsReady(p) : false;
        }

        public void SetInterstitialDelay(int delay)
        {
            _defaultInterstitialDelay = delay;
        }

        public void SetRewardedDelay(int delay)
        {
            _defaultRewardedDelay = delay;
        }

        public void SetInterstitialAfterRewardedDelay(int delay)
        {
            _defaultInterstitialAfterRewardedDelay = delay;
        }

        public async Task<bool> Request(string placementString, float timeout)
        {
            if (isInitialized == false) return false;

            var placement = GetPlacement(placementString);
            if (placement == null) return false;
            if (IsReady(placementString)) return true;
            if (removedAds && placement.type != AdType.Rewarded) return false;

#if UNITY_EDITOR
            await AsyncHelper.Delay();
#endif

            adProvider.Request(placement.type);

            while (IsReady(placementString) == false)
            {
                timeout -= Time.deltaTime;
                await AsyncHelper.Delay();

                if (timeout <= 0) break;

#if UNITY_EDITOR

                if (Application.isPlaying == false)
                {
                    Debug.LogWarning("Application is not playing. Requesting ads is not possible.");
                    return false;
                }

#endif
            }

            return IsReady(placementString);
        }

        public bool CanShow(Placement placement)
        {
            if (isInitialized == false) return false;

            if (placement == null) return false;
            if (IsReady(placement) == false) return false;
            if (removedAds && placement.type != AdType.Rewarded) return false;

            return true;
        }

        public async Task<bool> TryShowPlacement(string placementString)
        {
            var placement = GetPlacement(placementString);

            if (CanShow(placement) == false)
            {
                return false;
            }

            return await ShowPlacement(placement);
        }

        private async Task<bool> ShowPlacement(Placement placement)
        {
            _lastAdShownSuccessfully = false;
            _showAdResultChanged = false;
            _rewardedStatusChanged = false;
            _rewardEarned = false;

            _currentRequestedForShowPlacement = placement;
            _currentPlacements[placement.type] = placement;

            adProvider.Show(placement.type);

            while (_showAdResultChanged == false) await AsyncHelper.Delay();
            if (placement.type == AdType.Rewarded) while (_rewardedStatusChanged == false) await AsyncHelper.Delay();

            _currentRequestedForShowPlacement = null;

            return _lastAdShownSuccessfully;
        }

        public void ShowMediationDebuger()
        {
#if !ADMOB && APPLOVIN
           if (maxSdkProvider == null || !maxSdkProvider.IsInitialized)
           {
               Debug.LogWarning("MAX SDK not initialized yet!");
               return;
           }
           
           maxSdkProvider.ShowMediationDebugger();
#else
            Debug.Log($"Mediation debugger is only Applovin MAX SDK feature!");
#endif
        }

        public string GetAndroidAdvertiserId()
        {
            string advertisingID = "";
            try
            {
                AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

                advertisingID = adInfo.Call<string>("getId").ToString();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"OnAdGetAdvertisingId  error: {e}");
            }
            return advertisingID;
        }

        public string GetIOSAdvertiserId()
        {
            string advertisingID = "";

            Application.RequestAdvertisingIdentifierAsync((string id, bool trackingEnabled, string error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log($"OnAdGetAdvertisingId id: {id} trackingEnabled: {trackingEnabled}");

                    advertisingID = id;
                    OnGetAdvertissingId?.Invoke();
                }
                else
                {
                    Debug.LogWarning($"OnAdGetAdvertisingId  error: {error}");
                }
            });

            return advertisingID;
        }

        public async void Initialize()
        {
#if UNITY_ANDROID
            advertisingId = GetAndroidAdvertiserId();
#else
            advertisingId = GetIOSAdvertiserId();
#endif

            if (_initializeOnSetConsent == false) SetConsent(false);

            while (_isAdTrackingConsentValueSet == false) await AsyncHelper.Delay();

            Settings settings = null;

#if ADMOB

#if UNITY_EDITOR || UNITY_ANDROID
            settings = _admob_Android;
#elif UNITY_IOS
            settings = admob_ios;
#endif

            if (adProvider == null) adProvider = gameObject.AddComponent<AdMobProvider>();
#endif

#if APPLOVINMAX
            settings = applovin;

            adProvider = gameObject.AddComponent<MaxSdkProvider>();
#endif

#if IRONSOURCE
            adProvider = new gameObject.IronSourceProvider();
#endif

#if CLEVERADS
            adProvider = new gameObject.CleverAdsProvider();
#endif

            adProvider.OnError += (adType, adUnit, error) => { OnAdError(adType, adUnit, error); };
            adProvider.OnLoad += (adType, adUnit) => { OnAdLoaded(adType, adUnit); };
            adProvider.OnShow += (adType, adUnit) => { OnAdOpen(GetCurrentPlacement(adType)); };
            adProvider.OnClose += (adType, adUnit) => { OnAdClose(GetCurrentPlacement(adType)); };
            adProvider.OnEarnReward += (adType, adUnit) => { OnAdReward(GetCurrentPlacement(adType)); };
            adProvider.OnPaid += (adType, adUnit, revenue) => { OnAdPaid(GetCurrentPlacement(adType), revenue); };

            adProvider.OnInitialize += () =>
            {
                _currentPlacements = new Dictionary<AdType, Placement>();

                foreach (var ad in adProvider.InitializedAdTypes) _currentPlacements.Add(ad, null);

                isInitialized = true;

                RequestAll();
            };

#if ADMOB
            adProvider.Initialize(_isTest, _adTrackingConsent, null, settings.appOpenId, settings.bannerId, settings.interstitialId, settings.rewardedId, null, null);
#endif

#if APPLOVINMAX
            adProvider.Initialize(isTest, consentEnabled, settings.ApiKey, settings.AppOpenId, settings.BannerId, settings.InterstitialId, settings.RewardedId, settings.RewardedInterstitialId, settings.MrecId);
#endif
        }

        public void RemoveAds(bool remove)
        {
            removedAds = remove;

            if (remove)
            {
                HideAll();
                DestroyAll();
            }

#if CleverAds
            var cleverads = (CleverAdsProvider)adProvider;
            cleverads.SetAppReturnAdsEnabled(!enable);
#endif

            Debug.Log("NOADS Remove ads: " + remove);
        }

        public void SetConsent(bool consent)
        {
            _adTrackingConsent = consent;
            _isAdTrackingConsentValueSet = true;
        }

        public void RequestAll()
        {
            if (isInitialized == false) return;

            foreach (var placement in placements) Request(placement);
        }

        public void HideAll()
        {
            foreach (var placement in placements) Hide(placement);
        }

        public void DestroyAll()
        {
            foreach (var placement in placements) Destroy(placement);
        }

        public void DropDelay(AdType adType)
        {
            switch (adType)
            {
                case AdType.Interstitial:
                    LastInterstitialShow = DateTime.MinValue;
                    break;
                case AdType.Rewarded:
                    LastRewardedShow = DateTime.MinValue;
                    break;
            }
        }

        public bool Skip(Placement placement)
        {
            if (isInitialized == false) return false;

            bool skip = _skipAd.Any(x => x == placement.type);

            if (skip) Debug.Log($"OnAdSkiped: {placement}, Skip ad types: {string.Join(',', _skipAd)}");

            return skip;
        }

        public bool IsEnabled(AdType adType)
        {
            if (_enabledAd.Contains(adType))
            {
                return true;
            }
            else
            {
                Debug.Log($"{adType} is disabled!");
                return false;
            }
        }

        public bool IsDelayed(Placement placement)
        {
            if (isInitialized == false) return false;

            switch (placement.type)
            {
                case AdType.AppOpen:

                    if ((DateTime.Now - LastInterstitialShow).TotalSeconds < 5.0f || (DateTime.Now - LastRewardedShow).TotalSeconds < 5.0f)
                    {
                        Debug.Log($"OnAdDelayed: {placement.type}");
                        return true;
                    }

                    break;
                case AdType.Interstitial:
                    if ((DateTime.Now - LastInterstitialShow).TotalSeconds < _defaultInterstitialDelay)
                    {
                        Debug.Log($"OnAdDelayed: {placement.type} Last interstitial displayed {(DateTime.Now - LastInterstitialShow).TotalSeconds}sec ago. Timeout: {_defaultInterstitialDelay}sec");
                        return true;
                    }

                    if ((DateTime.Now - LastRewardedShow).TotalSeconds < _defaultInterstitialAfterRewardedDelay)
                    {
                        Debug.Log($"OnAdDelayed: {placement.type} Last rewarded displayed {(DateTime.Now - LastRewardedShow).TotalSeconds}sec ago. Timeout: {_defaultInterstitialAfterRewardedDelay}sec");
                        return true;
                    }

                    break;
                case AdType.Rewarded:
                    if ((DateTime.Now - LastRewardedShow).TotalSeconds < _defaultRewardedDelay)
                    {
                        Debug.Log($"OnAdDelayed: {placement.type} Last rewarded displayed {(DateTime.Now - LastRewardedShow).TotalSeconds}sec ago. Timeout: {_defaultRewardedDelay}sec");
                        return true;
                    }
                    break;
            }

            return false;
        }

        public bool IsReady(Placement placement)
        {
            if (isInitialized == false) return false;
            if (IsEnabled(placement.type) == false) return false;
            if (Skip(placement)) return false;
            if (IsDelayed(placement)) return false;

            return adProvider.IsReady(placement.type);
        }

        public void Request(Placement placement)
        {
            if (isInitialized == false)
            {
                Debug.LogWarning($"Cannot request {placement.type}. Ads not initialized yet!");
                return;
            }

            if (IsReady(placement)) return;
            if (removedAds && placement.type != AdType.Rewarded) return;
            if (placement.type == AdType.AppOpen && _currentAppOpenDelay > 0) return;

            adProvider.Request(placement.type);
        }

        public void Hide(string placementString)
        {
            if (isInitialized == false) return;

            var placement = GetPlacement(placementString);

            adProvider.Hide(placement.type);
        }

        public void Hide(Placement placement)
        {
            if (isInitialized == false) return;

            adProvider.Hide(placement.type);
        }

        public void Destroy(string placementString)
        {
            if (isInitialized == false) return;

            var placement = GetPlacement(placementString);

            adProvider.Destroy(placement.type);
        }

        public void Destroy(Placement placement)
        {
            if (isInitialized == false) return;

            adProvider.Destroy(placement.type);
        }


        #region Callbacks
        private void OnAdPaid(Placement placement, Revenue revenue)
        {
            OnPaid?.Invoke(placement, revenue);
        }

        private void OnAdError(AdType adType, string adUnit, string errorMessage)
        {
            _lastAdShownSuccessfully = false;

            _rewardedStatusChanged = true;

            OnError?.Invoke(adType, errorMessage);

            Debug.LogWarning($"OnAdError: {adType} {adUnit} {errorMessage}");
        }

        private void OnAdLoaded(AdType adType, string adUnit)
        {
            Debug.Log($"OnAdLoaded: {adType} {adUnit}");
            OnLoaded?.Invoke(adType);
        }

        private void OnAdOpen(Placement placement)
        {
            if (placement.type != AdType.AppOpen && placement.type != AdType.Banner)
            {
                _currentAppOpenDelay = 5.0f;
            }

            _lastAdShownSuccessfully = true;

            switch (placement.type)
            {
                case AdType.Interstitial:
                    LastInterstitialShow = DateTime.Now;
                    break;
                case AdType.Rewarded:
                    LastRewardedShow = DateTime.Now;
                    break;
            }

            Debug.Log($"OnAdOpen: {placement.placement}");
            OnOpen?.Invoke(placement);
        }

        private void OnAdClicked(Placement placement)
        {
            Debug.Log($"OnAdClicked: {placement.placement}");
        }

        private void OnAdClose(Placement placement)
        {
            Debug.Log($"OnAdClose: {placement.placement}");
            OnClose?.Invoke(placement);

            placement.lastShow = DateTime.Now;

            if (placement.type == AdType.Rewarded)
            {
                GetReward(placement);
            }

            if (_showAdResultChanged && _currentRequestedForShowPlacement != null && _currentRequestedForShowPlacement.Equals(placement))
            {
                _showAdResultChanged = false;
            }

            RequestAll();
        }

        private async void GetReward(Placement placement)
        {
            float timeout = 2.0f;

            while (timeout > 0 && _rewardEarned == false)
            {
                if (_getRewardCancellationTokenSource.IsCancellationRequested)
                {
                    _getRewardCancellationTokenSource = new CancellationTokenSource();
                    break;
                }

                timeout -= Time.deltaTime;
                await AsyncHelper.Delay();
            }

            if (_rewardEarned) OnRewarded?.Invoke(placement);
            else OnRewardedFailed?.Invoke(placement);

            _rewardedStatusChanged = true;
        }

        private void OnAdReward(Placement placement)
        {
            Debug.Log($"OnAdReward: {placement.placement}");
            _rewardEarned = true;
        }

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AdsManager))]
    [ExecuteInEditMode]
    public class AdsControllerEditor : Editor
    {
        private static Provider _tempProvider;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (AdsManager)target;

#if ADMOB
            EditorGUILayout.HelpBox("Set app ID's in GoogleMobileAdsSettings", MessageType.Warning);
#endif

#if APPLOVINMAX
            EditorGUILayout.HelpBox("Set adapters and additionals ID's in Integration manager window", MessageType.Warning);
#endif

            EditorGUILayout.LabelField($"Version: {AdsManager.version}");

            if (Application.isPlaying == false & _tempProvider != script.provider)
            {
                _tempProvider = script.provider;

                script.ApplySdks();

                Debug.Log("Ad provider: " + _tempProvider);
            }

            //if (GUILayout.Button("Apply SDK's"))
            //{
            //    script.ApplySdks();
            //}
        }
    }
#endif

    [Serializable]
    public class Revenue
    {
        public string provider;
        public string adUnit;
        public string placement;
        public string countryCode;
        public string network;
        public double value;
        public string currencyCode;
    }

    [Serializable]
    public class Settings
    {
#if !ADMOB
        [field: SerializeField] public string ApiKey { get; private set; }
#endif

        [field: SerializeField] public string appOpenId { get; private set; }
        [field: SerializeField] public string interstitialId { get; private set; }
        [field: SerializeField] public string rewardedId { get; private set; }

#if !ADMOB
        [field: SerializeField] public string RewardedInterstitialId { get; private set; }
#endif

        [field: SerializeField] public string bannerId { get; private set; }


#if !ADMOB
        [field: SerializeField] public string MrecId { get; private set; }
#endif

        [field: SerializeField] public string adapterDebug { get; private set; }
    }
}