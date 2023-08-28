using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Managers
{
    public class RemoteConfigManager : MonoBehaviour
    {
        private const string _defaultMouseSensitivityString = "DefMouseSens";
        private const string _defaultWinPercentageString = "WinPerce";
        private const string _defaultLosePercentageString = "LosePerce";

        public struct UserAttributes { }
        public struct AppAttributes { }

        public async void Initialize()
        {
            if (Utilities.CheckForInternetConnection() == true)
            {
                await InitializeRemoteConfigAsync();
            }

            RemoteConfigService.Instance.FetchCompleted -= ApplyRemoteConfig;
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

#if UNITY_EDITOR
            RemoteConfigService.Instance.SetEnvironmentID("development");
#else
            RemoteConfigService.Instance.SetEnvironmentID("production");
#endif

            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }

        private void OnDestroy()
        {
            RemoteConfigService.Instance.FetchCompleted -= ApplyRemoteConfig;
        }

        private async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn == false)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private void ApplyRemoteConfig(ConfigResponse configResponse)
        {
            RemoteConfigService.Instance.FetchCompleted -= ApplyRemoteConfig;

            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("No settings loaded this session and no local cache file exists; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("New settings loaded this session; update values accordingly.");
                    break;
            }

            Debug.Log(RemoteConfigService.Instance.appConfig.GetFloat(_defaultMouseSensitivityString));
            Debug.Log(RemoteConfigService.Instance.appConfig.GetFloat(_defaultWinPercentageString));
            Debug.Log(RemoteConfigService.Instance.appConfig.GetFloat(_defaultLosePercentageString));
        }
    }
}