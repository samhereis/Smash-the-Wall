using DTO;
using DTO.Save;
using Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Weapons;

namespace IdentityCards
{
    [Serializable]
    public class WeaponIdentityiCard : IdentityCardBase<WeaponBase>, IInitializable<AWeapon_DTO>
    {
        [Required]
        [field: SerializeField] public Sprite icon { get; private set; }

        [field: SerializeField] public bool isDefault { get; private set; } = false;
        [field: SerializeField] public bool isUnlocked { get; private set; } = false;
        [field: SerializeField] public int opensAtLevel { get; private set; } = 0;

        public void Initialize(AWeapon_DTO weapon_DTO)
        {
            Validate();

            if (isDefault == true)
            {
                SetIsUnlockedStatus(true);
            }
            else
            {
                SetIsUnlockedStatus(weapon_DTO.isUnlocked);
            }
        }

        public void SetIsUnlockedStatus(bool isUnlocked)
        {
            this.isUnlocked = isUnlocked;
        }

        public bool IsToUnlock(LevelSave_DTO levelSave)
        {
            return levelSave.levelIndex >= opensAtLevel && isUnlocked == false;
        }
    }
}