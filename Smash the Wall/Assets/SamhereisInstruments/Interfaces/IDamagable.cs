using Identifiers;
using System;

namespace Interfaces
{
    public interface IDamagable
    {
        public Action onDie { get; set; }

        public IdentifierBase damagedGameobject { get; }
        public float currentHealth { get; }
        public float maxHealth { get; }
        public bool isAlive { get; }

        public float TakeDamage(float damage, IDamager damager);
    }
}