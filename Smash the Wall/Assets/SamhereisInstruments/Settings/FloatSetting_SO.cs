using Helpers;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Sensitivity", menuName = "Scriptables/Settings/FloatSetting")]
    public class FloatSetting_SO : ScriptableObject
    {
        [SerializeField] private float _currentValue;
        public float currentValue => _currentValue;

#if UNITY_EDITOR
        private string _keyStartsWith = "";
        private string _keyEndsWith = "_Key";
#endif

        [SerializeField] private string _KEY;
        public string KEY => _KEY;

        [SerializeField] private float _defaultValue = 0.5f;

        private void OnValidate()
        {
            _currentValue = PlayerPrefs.GetFloat(KEY, _defaultValue);

#if UNITY_EDITOR

            if (name.StartsWith("_Key") == false)
            {
                _KEY = _keyStartsWith + name + _keyEndsWith;
                this.TrySetDirty();
            }

#endif

        }

        private void OnEnable()
        {
            _currentValue = PlayerPrefs.GetFloat(KEY, _defaultValue);
        }

        public void SetData(float value)
        {
            _currentValue = value;

            PlayerPrefs.SetFloat(KEY, _currentValue);
            PlayerPrefs.Save();
        }
    }
}