using System;
using UnityEngine;

namespace IdentityCards
{
    [Serializable]
    public abstract class IdentityCardBase<T>
    {
        [field: SerializeField] public string targetName { get; protected set; }
        [field: SerializeField] public T target { get; protected set; }

        public void SetTargetName(string targetName)
        {
            this.targetName = targetName;
        }

        public void SetTarget(T target)
        {
            this.target = target;
        }

        public void AutoSetTargetName()
        {
            targetName = target.ToString();
        }
    }
}