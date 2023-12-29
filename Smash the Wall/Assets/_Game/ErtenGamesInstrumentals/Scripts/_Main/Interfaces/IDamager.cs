using Identifiers;

namespace Interfaces
{
    public interface IDamager : IInitializable<IdentifierBase>
    {
        public IDamagerActor damagerActor { get; }
        public IdentifierBase damagerGameobject { get; }
        public float Damage(IDamagable damagable, float damage);
    }
}