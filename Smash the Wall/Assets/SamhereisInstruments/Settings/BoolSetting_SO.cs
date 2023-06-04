using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BoolSetting_SO", menuName = "Scriptables/Settings/BoolSetting_SO")]
    public sealed class BoolSetting_SO : ScriptableObject
    {
        [SerializeField] private bool _currentValue;
        public bool currentValue => _currentValue;

#if UNITY_EDITOR
        private string _keyStartsWith = "";
        private string _keyEndsWith = "_Key";
#endif

        [SerializeField] private string _KEY;
        public string KEY => _KEY;

        [SerializeField] private string _defaultValue = "true";
        [SerializeField] private string _trueValue = "true";
        [SerializeField] private string _falseValue = "false";

        private void OnValidate()
        {
            _currentValue = PlayerPrefs.GetString(KEY, _defaultValue) == _trueValue;

#if UNITY_EDITOR
            if (name.StartsWith("_Key") == false) _KEY = _keyStartsWith + name + _keyEndsWith;
#endif
        }

        private void OnEnable()
        {
            _currentValue = PlayerPrefs.GetString(KEY, _defaultValue) == _trueValue;
        }

        public void SetData(bool value)
        {
            _currentValue = value;

            PlayerPrefs.SetString(KEY, value ? _trueValue : _falseValue);
            PlayerPrefs.Save();
        }
    }
}