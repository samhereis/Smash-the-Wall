#if RemoteConfigInstalled

using DependencyInjection;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Services
{
    public abstract class RemoteConfigsServiceBase: MonoBehaviour, IDIDependent
    {
        public struct UserAttributes { }
        public struct AppAttributes { }

        public async void Initialize()
        {
            if (Utilities.CheckForInternetConnection() == false)
            {
                return;
            }

            await InitializeRemoteConfigAsync();

            RemoteConfigService.Instance.FetchCompleted -= OnFetched;
            RemoteConfigService.Instance.FetchCompleted += OnFetched;

#if UNITY_EDITOR
            RemoteConfigService.Instance.SetEnvironmentID("development");
#else
            RemoteConfigService.Instance.SetEnvironmentID("production");
#endif

            await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        }

        private void OnDestroy()
        {
            RemoteConfigService.Instance.FetchCompleted -= OnFetched;
        }

        private async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();

            if (AuthenticationService.Instance.IsSignedIn == false)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        protected virtual void OnFetched(ConfigResponse configResponse)
        {
            DependencyInjector.InjectDependencies(this);

            RemoteConfigService.Instance.FetchCompleted -= OnFetched;

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
        }
    }
}

#endif