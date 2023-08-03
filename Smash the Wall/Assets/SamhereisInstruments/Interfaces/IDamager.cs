using Identifiers;

namespace Interfaces
{
    public interface IDamager : IInitializable<IdentifierBase>
    {
        public IdentifierBase damagerGameobject { get; }
        public float Damage(IDamagable damagable, float damage);
    }
}