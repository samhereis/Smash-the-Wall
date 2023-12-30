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
        [field: SerializeField, HorizontalGroup("Row1"), VerticalGroup("Row1/Column1")]
        public bool isDefault { get; private set; } = false;

        [field: SerializeField, HorizontalGroup("Row1"), VerticalGroup("Row1/Column1")]
        public bool isUnlocked { get; private set; } = false;

        [field: SerializeField, HorizontalGroup("Row1"), VerticalGroup("Row1/Column1")]
        public int opensAtLevel { get; private set; } = 0;

        [Required]
        [field: SerializeField, HorizontalGroup("Row1", width: 0.2f), VerticalGroup("Row1/Column2"), PreviewField(Height = 55), HideLabel]
        public Sprite icon { get; private set; }

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