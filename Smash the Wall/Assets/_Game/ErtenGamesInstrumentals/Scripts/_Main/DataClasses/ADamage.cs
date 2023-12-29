using Interfaces;

namespace DataClasses
{
    public struct ADamage
    {
        public IDamager damagerObject;
        public IDamagable damagedObject;

        public float damageAmount;
        public float healthAfterDamage;
    }
}
