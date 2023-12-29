using Interfaces;
using System.Collections.Generic;

namespace Services
{
    public abstract class AnalyticsServiceBase : IInitializable
    {
        public bool isDataCollectionEnabled { get; protected set; }

        public virtual void Initialize()
        {

        }

        public virtual void SetDataCollectionStatus(bool collect)
        {
            isDataCollectionEnabled = collect;
        }

        public virtual void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public virtual void LogEvent(string name, string parameterName, float parameterValue)
        {
            if (isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public virtual void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (isDataCollectionEnabled == false) return;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { parameterName, parameterValue}
            };

            LogEvent(name, parameters);
        }

        public abstract void LogEvent(string name);

        public abstract void LogEvent(string name, Dictionary<string, object> parameters);
    }
}