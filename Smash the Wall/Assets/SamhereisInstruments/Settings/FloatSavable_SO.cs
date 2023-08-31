using Helpers;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "FloatSavable_SO", menuName = "Scriptables/Settings/FloatSavable_SO")]
    public class FloatSavable_SO : ScriptableObject
    {
        [SerializeField] private float _currentValue;
        public float currentValue => _currentValue;

#if UNITY_EDITOR
        private string _keyStartsWith = "";
        private string _keyEndsWith = "_key";
#endif

        [SerializeField] private string _key;
        public string key => _key;

        [SerializeField] private float _defaultValue = 0.5f;

        private void OnValidate()
        {
            _currentValue = PlayerPrefs.GetFloat(key, _defaultValue);

#if UNITY_EDITOR

            if (name.StartsWith("_Key") == false)
            {
                _key = _keyStartsWith + name + _keyEndsWith;
                this.TrySetDirty();
            }

#endif

        }

        private void OnEnable()
        {
            _currentValue = PlayerPrefs.GetFloat(key, _defaultValue);
        }

        public void SetData(float value)
        {
            _currentValue = value;

            PlayerPrefs.SetFloat(key, _currentValue);
            PlayerPrefs.Save();
        }
    }
}