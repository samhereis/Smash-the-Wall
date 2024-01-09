using Interfaces;
using System;
using UnityEngine;

namespace Observables
{
    [Serializable]
    public class Signal : IInitializable<string>
    {
        public Action onInvoke { get; private set; }

        [SerializeField] public string eventName { get; private set; }
        [SerializeField] public int currentSubscribedObjectsCount { get; private set; }
        [SerializeField] public int timesInvoked { get; private set; }

        public Signal(string eventName)
        {
            Initialize(eventName);
        }

        public void Initialize(string eventName)
        {
            this.eventName = eventName;
        }

        public void AddListener(Action action)
        {
            onInvoke += action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void RemoveListener(Action action)
        {
            onInvoke -= action;
            UpdateCurrentSubscribedObjectsCount();
        }

        public void Clear()
        {
            onInvoke = null;
        }

        public void Invoke()
        {
            onInvoke?.Invoke();
            timesInvoked++;

            Debug.Log("Invoked: " + eventName);
        }

        private void UpdateCurrentSubscribedObjectsCount()
        {
            if (onInvoke == null)
            {
                currentSubscribedObjectsCount = 0;
            }
            else
            {
                currentSubscribedObjectsCount = onInvoke.GetInvocationList().Length;
            }
        }
    }
}