using UnityEngine;

namespace IdentityCards
{
    public abstract class IdentityCardBase<T>
    {
        [field: SerializeField] public string targetName { get; protected set; }
        [field: SerializeField] public T target { get; protected set; }
    }
}
