using Sirenix.OdinInspector;
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
            if (data == null)
            {
                result.AddWarning("Data is null").WithFix(() =>
                {
                    data = new List<SimpleSound>();
                });
            }

            if (data.Count == 0)
            {
                result.AddWarning("Sound list is empty");
            }
        }
    }
}