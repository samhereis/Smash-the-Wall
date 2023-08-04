using DTO.Save;
using Interfaces;
using System;
using UnityEngine;
using Weapons;

namespace IdentityCards
{
    [Serializable]
    public class WeaponIdentityiCard : IdentityCardBase<WeaponBase>, IInitializable<Weapons_DTO>
    {
        [field: SerializeField] public bool isUnlocked { get; private set; } = false;

        public void Initialize(Weapons_DTO weaposSave)
        {
            if (weaposSave == null)
            {
                return;
            }

            var aWeapon = weaposSave.allWeapons.Find(x => x.weaponName == targetName);

            if (aWeapon == null)
            {
                return;
            }

            SetIsUnlockedStatus(aWeapon.isUnlocked);
        }

        public void SetIsUnlockedStatus(bool isUnlocked)
        {
            this.isUnlocked = isUnlocked;
        }
    }
}