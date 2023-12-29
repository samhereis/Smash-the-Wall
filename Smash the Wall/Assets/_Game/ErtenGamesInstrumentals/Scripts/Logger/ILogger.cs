using UnityEngine;

namespace Loggers
{
    public interface ILogger
    {
        public bool enableLogs { get; }

        public void LogInfoToConsole(string message, Object context);
        public void LogWarningToConsole(string message, Object context);
        public void LogErrorToConsole(string message, Object context);
    }
}