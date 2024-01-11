using System;
using UnityEngine;

namespace Observables
{
    [Serializable]
    public class ObservableValue<T>
    {
        [SerializeField] private T _value;

        [field: SerializeField] public string eventName { get; private set; }
        [field: SerializeField] public int currentSubscribedObjectsCount { get; private set; }
        [field: SerializeField] public int timesInvoked { get; private set; }

        [SerializeField] protected Action<T> onValueChange { get; set; }

        public T value
        {
            get { return _value; }
            set { ChangeValue(value); }
        }

        public ObservableValue(string eventName)
        {
            this.eventName = eventName;
        }

        public virtual void AddListener(Action<T> listener)
        {
            onValueChange += listener;
            UpdateCurrentSubscribedObjectsCount();
        }

        public virtual void RemoveListener(Action<T> listener)
        {
            onValueChange -= listener;
            UpdateCurrentSubscribedObjectsCount();
        }

        public virtual void ChangeValue(T sentValue)
        {
            value = sentValue;
            onValueChange?.Invoke(sentValue);

            timesInvoked++;
        }

        private void UpdateCurrentSubscribedObjectsCount()
        {
            if (onValueChange == null)
            {
                currentSubscribedObjectsCount = 0;
            }
            else
            {
                currentSubscribedObjectsCount = onValueChange.GetInvocationList().Length;
            }
        }
    }
}