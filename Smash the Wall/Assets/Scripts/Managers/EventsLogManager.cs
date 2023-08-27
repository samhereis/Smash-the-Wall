using Firebase.Analytics;
using UnityEngine;

namespace Managers
{
    public class EventsLogManager : MonoBehaviour
    {
        public static void LogEvent(string name, string parameterName, string parameterValue)
        {
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public static void LogEvent(string name, string parameterName, double parameterValue)
        {
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public static void LogEvent(string name, string parameterName, long parameterValue)
        {
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public static void LogEvent(string name, string parameterName, int parameterValue)
        {
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public static void LogEvent(string name)
        {
            FirebaseAnalytics.LogEvent(name);
        }

        public static void LogEvent(string name, params Parameter[] parameters)
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }
}