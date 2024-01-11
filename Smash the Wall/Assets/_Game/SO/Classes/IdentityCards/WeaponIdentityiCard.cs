using DTO.Save;
using ErtenGamesInstrumentals.DataClasses;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Weapons;

namespace IdentityCards
{
    [Serializable]
    public class WeaponIdentityiCard : PrefabReference<WeaponBase>
    {
        [field: SerializeField] public bool isDefault { get; private set; } = false;
        [field: SerializeField] public int opensAtLevel { get; private set; } = 0;

        [Required]
        [field: SerializeField] public Sprite icon { get; private set; }

        [field: SerializeField, ReadOnly] public bool isUnlocked { get; private set; } = false;
        [field: SerializeField, ReadOnly] public string targetName { get; private set; }

        public WeaponIdentityiCard(WeaponBase resourceReference) : base(resourceReference)
        {

        }

        public override async void Setup()
        {
            base.Setup();

            var target = await GetAssetAsync();
            targetName = target.name;
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