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
        private bool _isDataCollectionEnabled = false;

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

                if (collect == true)
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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name, string parameterName, float parameterValue)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name, string parameterName, int parameterValue)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public static void LogEvent(string name)
        {
            try
            {
                FirebaseAnalytics.LogEvent(name);

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
            try
            {
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