using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BoolSavable_SO", menuName = "Scriptables/Settings/BoolSavable_SO")]
    public sealed class BoolSavable_SO : BaseSavable_SO<bool>, ISelfValidator
    {
        public bool currentValue => _currentValue;

        public void Validate(SelfValidationResult result)
        {
            Initialize();

#if UNITY_EDITOR

            if (name.StartsWith("_Key") == false)
            {
                _key = _keyStartsWith + name + _keyEndsWith;
                this.TrySetDirty();
            }

#endif
        }

        [Button]
        public override void Initialize()
        {
            _currentValue = PlayerPrefs.GetString(_key, _defaultValue.ToString()) == true.ToString();
        }

        public override void SetData(bool value)
        {
            base.SetData(value);

            PlayerPrefs.SetString(_key, value.ToString());
            PlayerPrefs.Save();
        }
    }
}