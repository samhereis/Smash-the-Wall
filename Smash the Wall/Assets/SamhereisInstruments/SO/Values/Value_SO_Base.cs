using UnityEngine;
using UnityEngine.Events;

namespace Values
{
    public abstract class Value_SO_Base<T> : ScriptableObject
    {
        [field: SerializeField] public virtual T value { get; protected set; }

        [SerializeField] protected UnityEvent<T> onValueChange { get; set; } = new UnityEvent<T>();

        public virtual void AddListener(UnityAction<T> listener)
        {
            onValueChange.AddListener(listener);
        }

        public virtual void RemoveListener(UnityAction<T> listener)
        {
            onValueChange.RemoveListener(listener);
        }

        public virtual void ChangeValue(T sentValue)
        {
            value = sentValue;
            onValueChange.Invoke(sentValue);
        }
    }
}