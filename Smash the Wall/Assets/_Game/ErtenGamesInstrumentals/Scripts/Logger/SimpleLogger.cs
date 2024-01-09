using UnityEngine;

namespace Loggers
{
    public class SimpleLogger : LoggerBase
    {
        [Header("Settings")]
        [SerializeField] private string _prefix = string.Empty;

        private void Reset()
        {
            _prefix = gameObject.name;
        }

        public override void Log(string message, Object context)
        {
            if (enableLogs == false) { return; }

            Debug.Log(_prefix + ": " + message, context);
        }

        public override void LogWarning(string message, Object context)
        {
            if (enableLogs == false) { return; }

            Debug.LogWarning(_prefix + ": " + message, context);
        }

        public override void LogError(string message, Object context)
        {
            if (enableLogs == false) { return; }

            Debug.LogError(_prefix + ": " + message, context);
        }
    }
}