using UnityEngine;

namespace SO.DataHolders
{
    //[CreateAssetMenu(fileName = "ADataHolder", menuName = "Scriptables/DataHolders/ADataHolder")]
    public class DataHolder_Base<T> : ScriptableObject
    {
        [field: SerializeField] public T data = default(T);
    }
}