using DTO.Save;
using Interfaces;
using ProjectSripts;
using System;
using UnityEngine;
using Weapons;

namespace IdentityCards
{
    [Serializable]
    public class WeaponIdentityiCard : IdentityCardBase<WeaponBase>, IInitializable<AWeapon_DTO>
    {
        [field: SerializeField] public bool isDefault { get; private set; } = false;
        [field: SerializeField] public bool isUnlocked { get; private set; } = false;

        public void Initialize(AWeapon_DTO weapon_DTO)
        {
            if (targetName == string.Empty)
            {
                AutoSetTargetName();
            }

            SetIsUnlockedStatus(weapon_DTO.isUnlocked);
        }

        public void SetIsUnlockedStatus(bool isUnlocked)
        {
            this.isUnlocked = isUnlocked;
        }
    }
}