using DTO;
using Interfaces;
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
        [field: SerializeField] public int opensAtLevel { get; private set; } = 0;
        [field: SerializeField] public Sprite icon { get; private set; }

        public void Initialize(AWeapon_DTO weapon_DTO)
        {
            AutoSetTargetName();

            SetIsUnlockedStatus(weapon_DTO.isUnlocked);
        }

        public void SetIsUnlockedStatus(bool isUnlocked)
        {
            this.isUnlocked = isUnlocked;
        }
    }
}