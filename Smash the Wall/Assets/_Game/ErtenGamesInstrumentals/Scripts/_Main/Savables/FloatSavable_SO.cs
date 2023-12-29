using Helpers;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "FloatSavable_SO", menuName = "Scriptables/Settings/FloatSavable_SO")]
    public class FloatSavable_SO : BaseSavable_SO<float>
    {
        public float currentValue => _currentValue;
        public string key => _key;


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

        public override void Initialize()
        {
            _currentValue = PlayerPrefs.GetFloat(key, _defaultValue);
        }

        public override void SetData(float value)
        {
            base.SetData(value);

            PlayerPrefs.SetFloat(key, _currentValue);
            PlayerPrefs.Save();
        }
    }
}