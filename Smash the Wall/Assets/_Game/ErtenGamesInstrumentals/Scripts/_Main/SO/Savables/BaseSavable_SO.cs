using Interfaces;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BaseSavable_SO", menuName = "Scriptables/Settings/BaseSavable_SO")]
    public class BaseSavable_SO<T> : ScriptableObject, IInitializable
    {
        [SerializeField] protected T _currentValue;
        [SerializeField] protected T _defaultValue = default(T);
        [SerializeField] protected string _key;

#if UNITY_EDITOR

        protected string _keyStartsWith = "";
        protected string _keyEndsWith = "_key";

#endif

        protected virtual void OnEnable()
        {
            Initialize();
        }

        public virtual void Initialize()
        {

        }

        public virtual void SetData(T value)
        {
            _currentValue = value;
        }
    }
}