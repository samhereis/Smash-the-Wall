using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sound;
using System.Collections.Generic;
using UnityEngine;

namespace SO.DataHolders
{
    [CreateAssetMenu(fileName = "SoundsPack_DataHolder", menuName = "Scriptables/DataHolders/SoundsPack_DataHolder")]
    public class SoundsPack_DataHolder : DataHolder_Base<List<SimpleSound>>, ISelfValidator
    {
        public void Validate(SelfValidationResult result)
        {
            if (data.IsNullOrEmpty())
            {
                result.AddWarning("Data is null").WithFix(() =>
                {
                    data = new List<SimpleSound>();
                    data.Add(new SimpleSound());
                });
            }
        }
    }
}