using UnityEngine;

namespace Loggers
{
    public abstract class LoggerBase : MonoBehaviour, ILogger
    {
        [SerializeField] public bool enableLogs { get; private set; }

        public virtual void Log(string message, Object context)
        {
            Debug.Log(message, context);
        }

        public virtual void LogWarning(string message, Object context)
        {
            Debug.LogWarning(message, context);
        }

        public virtual void LogError(string message, Object context)
        {
            Debug.LogError(message, context);
        }
    }
}