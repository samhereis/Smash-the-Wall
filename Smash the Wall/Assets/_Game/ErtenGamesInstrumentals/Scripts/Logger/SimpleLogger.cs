using UnityEngine;

namespace Loggers
{
    public class SimpleLogger : LoggerBase
    {
        [Header("Settings")]
        [SerializeField] private string _prefix = string.Empty;

        public override void Log(string message, Object context)
        {
            Debug.Log(_prefix + ": " + message, context);
        }

        public override void LogWarning(string message, Object context)
        {
            Debug.LogWarning(_prefix + ": " + message, context);
        }

        public override void LogError(string message, Object context)
        {
            Debug.LogError(_prefix + ": " + message, context);
        }
    }
}