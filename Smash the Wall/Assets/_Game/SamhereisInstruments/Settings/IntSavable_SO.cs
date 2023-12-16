using Helpers;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "FloatSavable_SO", menuName = "Scriptables/Settings/FloatSavable_SO")]
    public class IntSavable_SO : ScriptableObject
    {
        [SerializeField] private int _currentValue;
        public int currentValue => _currentValue;

#if UNITY_EDITOR
        private string _keyStartsWith = "";
        private string _keyEndsWith = "_key";
#endif

        [SerializeField] private string _key;
        public string key => _key;

        [SerializeField] private int _defaultValue = 0;

        private void OnValidate()
        {
            _currentValue = PlayerPrefs.GetInt(key, _defaultValue);

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
            _currentValue = PlayerPrefs.GetInt(key, _defaultValue);
        }

        public void SetData(int value)
        {
            _currentValue = value;

            PlayerPrefs.SetInt(key, _currentValue);
            PlayerPrefs.Save();
        }
    }
}