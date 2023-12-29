using Sirenix.OdinInspector;
using UnityEngine;

namespace Loggers
{
    public class SimpleLogger : MonoBehaviour, ILogger
    {
        [ShowInInspector] public bool enableLogs { get; private set; }

        [Header("Settings")]
        [ShowInInspector] private string _prefix = string.Empty;

        public void LogInfoToConsole(string message, Object context)
        {
            Debug.Log(_prefix + ": " + message, context);
        }

        public void LogWarningToConsole(string message, Object context)
        {
            Debug.LogWarning(_prefix + ": " + message, context);
        }

        public void LogErrorToConsole(string message, Object context)
        {
            Debug.LogError(_prefix + ": " + message, context);
        }
    }
}