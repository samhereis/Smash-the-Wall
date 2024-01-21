using UnityEngine;

namespace SO.DataHolders
{
    public abstract class DataHolder_Base<T> : ScriptableObject
    {
        [field: SerializeField] public virtual T data { get; protected set; } = default(T);
    }
}