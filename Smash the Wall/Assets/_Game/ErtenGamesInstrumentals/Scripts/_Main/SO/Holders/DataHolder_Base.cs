using UnityEngine;

namespace SO.DataHolders
{
    //[CreateAssetMenu(fileName = "ADataHolder", menuName = "Scriptables/DataHolders/ADataHolder")]
    public abstract class DataHolder_Base<T> : ScriptableObject
    {
        [field: SerializeField] public virtual T data { get; protected set; } = default(T);
    }
}