using UnityEngine;

namespace Loggers
{
    public interface ILogger
    {
        public bool enableLogs { get; }

        public void Log(string message, Object context = null);
        public void LogWarning(string message, Object context = null);
        public void LogError(string message, Object context = null);
    }
}