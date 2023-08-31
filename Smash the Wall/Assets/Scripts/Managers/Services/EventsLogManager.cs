using Firebase.Analytics;
using Interfaces;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace Managers
{
    public class EventsLogManager : MonoBehaviour, IInitializable
    {
        private static bool _isDataCollectionEnabled = false;

        public async void Initialize()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public void SetDataCollectionStatus(bool collect)
        {
            try
            {
                if (collect == _isDataCollectionEnabled) return;
                _isDataCollectionEnabled = collect;

                if (_isDataCollectionEnabled == true)
                {
                    AnalyticsService.Instance.StartDataCollection();
                }
                else
                {
                    AnalyticsService.Instance.StopDataCollection();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (_isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name, string parameterName, float parameterValue)
        {
            if (_isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (_isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name)
        {
            if (_isDataCollectionEnabled == false) return;

            try
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(name);

                AnalyticsService.Instance.CustomData(name);
                AnalyticsService.Instance.Flush();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static void LogEvent(string name, Dictionary<string, object> parameters)
        {
            if (_isDataCollectionEnabled == false) return;

            try
            {
                List<Parameter> fbParameters = new List<Parameter>();

                foreach (var parameter in parameters)
                {
                    fbParameters.Add(new Parameter(parameter.Key, parameter.Value.ToString()));
                }

                FirebaseAnalytics.LogEvent(name, fbParameters.ToArray());

                AnalyticsService.Instance.CustomData(name, parameters);
                AnalyticsService.Instance.Flush();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}