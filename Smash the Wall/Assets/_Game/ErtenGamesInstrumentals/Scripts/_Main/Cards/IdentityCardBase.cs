using Sirenix.OdinInspector;
using System;

namespace IdentityCards
{
    [Serializable]
    public abstract class IdentityCardBase<T>
    {
        [ShowInInspector] public string targetName { get; protected set; }
        [ShowInInspector] public T target { get; protected set; }

        public void SetTargetName(string targetName)
        {
            this.targetName = targetName;
        }

        public void SetTarget(T target, bool autoSetTargetName = true)
        {
            this.target = target;
            if (autoSetTargetName == true) Validate();
        }

        public virtual void Validate()
        {
            if (this.target != null)
            {
                targetName = target.ToString();
            }
        }
    }
}