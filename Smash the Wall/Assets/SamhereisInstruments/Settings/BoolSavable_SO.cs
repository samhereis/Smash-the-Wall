using Helpers;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BoolSavable_SO", menuName = "Scriptables/Settings/BoolSavable_SO")]
    public sealed class BoolSavable_SO : ScriptableObject
    {
        [SerializeField] private bool _currentValue;
        public bool currentValue => _currentValue;

#if UNITY_EDITOR
        private string _keyStartsWith = "";
        private string _keyEndsWith = "_key";
#endif

        [SerializeField] private string _key;
        public string key => _key;

        [SerializeField] private string _defaultValue = "true";
        [SerializeField] private string _trueValue = "true";
        [SerializeField] private string _falseValue = "false";

        private void OnValidate()
        {
            _currentValue = PlayerPrefs.GetString(key, _defaultValue) == _trueValue;

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
            _currentValue = PlayerPrefs.GetString(key, _defaultValue) == _trueValue;
        }

        public void SetData(bool value)
        {
            _currentValue = value;

            PlayerPrefs.SetString(key, value ? _trueValue : _falseValue);
            PlayerPrefs.Save();
        }
    }
}