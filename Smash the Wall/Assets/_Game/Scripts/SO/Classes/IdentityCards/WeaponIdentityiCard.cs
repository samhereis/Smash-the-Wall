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
        [ShowInInspector] public bool isUnlocked => IsUnclocked();

        [field: SerializeField] public bool isDefault { get; private set; } = false;
        [field: SerializeField] public int opensAtLevel { get; private set; } = 0;
        [SerializeField] private bool _isUnlocked = false;

        [Required]
        [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField, ReadOnly] public string targetName { get; private set; }

        private bool IsUnclocked()
        {
            return _isUnlocked || isDefault;
        }

        public bool IsToUnlock(LevelSave_DTO levelSave)
        {
            return levelSave.levelIndex >= opensAtLevel && isUnlocked == false;
        }

        public override async void Setup()
        {
            base.Setup();

            var target = await GetAssetAsync();
            targetName = target.name;
        }

        public void SetIsUnlockedStatus(bool isUnlocked)
        {
            _isUnlocked = isUnlocked;
        }
    }
}