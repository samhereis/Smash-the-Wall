using Configs;
using DI;
using InGameStrings;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Managers
{
    public class RemoteConfigManager : MonoBehaviour, IDIDependent
    {
        private const string _uiAnimationElementForeachDelayString = "uiAnimationElementForeachDelay";
        private const string _winPercentageString = "WinPerce";
        private const string _losePercentageString = "LosePerce";

        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;
        [DI(DIStrings.uiConfigs)][SerializeField] private UIConfigs _uIConfigs;

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
            (this as IDIDependent).LoadDependencies();

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

            _gameConfigs.gameplaySettings.SetWinPercentage(RemoteConfigService.Instance.appConfig.GetFloat(_winPercentageString));
            _gameConfigs.gameplaySettings.SetLosePercentage(RemoteConfigService.Instance.appConfig.GetFloat(_losePercentageString));

            _uIConfigs.SetUIAnimationElementForeachDelay(RemoteConfigService.Instance.appConfig.GetFloat(_uiAnimationElementForeachDelayString));
        }
    }
}