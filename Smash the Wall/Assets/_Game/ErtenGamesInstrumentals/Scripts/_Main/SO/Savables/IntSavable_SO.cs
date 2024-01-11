using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "FloatSavable_SO", menuName = "Scriptables/Settings/FloatSavable_SO")]
    public class IntSavable_SO : BaseSavable_SO<int>, ISelfValidator
    {
        public int currentValue => _currentValue;
        public string key => _key;

        public void Validate(SelfValidationResult result)
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

        public override void Initialize()
        {
            _currentValue = PlayerPrefs.GetInt(key, _defaultValue);
        }

        public override void SetData(int value)
        {
            base.SetData(value);

            PlayerPrefs.SetInt(key, _currentValue);
            PlayerPrefs.Save();
        }
    }
}