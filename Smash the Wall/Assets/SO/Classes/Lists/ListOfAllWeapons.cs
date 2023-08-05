using Configs;
using IdentityCards;
using Managers;
using ProjectSripts;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllWeapons", menuName = "SO/Lists/ListOfAllWeapons")]
    public class ListOfAllWeapons : ConfigBase
    {
        [field: SerializeField] public List<WeaponIdentityiCard> weapons { get; private set; } = new List<WeaponIdentityiCard>();
        [field: SerializeField] public WeaponIdentityiCard defaultWeapon { get; private set; }

        public override void Initialize()
        {
            var weaponsSave = GameSaveManager.GetWeaponsSave();

            InitWeapon(defaultWeapon);

            foreach (var weaponsIdentifier in weapons)
            {
                InitWeapon(weaponsIdentifier);
            }

            void InitWeapon(WeaponIdentityiCard weaponIdentityiCard, bool isUnlockedValueIfNotFoundInSaveFile = false)
            {
                if (weaponsSave.allWeapons.Find(x => x.weaponName == weaponIdentityiCard.targetName) == null)
                {
                    weaponsSave.allWeapons.Add(new AWeapon_DTO
                    {
                        weaponName = weaponIdentityiCard.targetName,
                        isUnlocked = isUnlockedValueIfNotFoundInSaveFile
                    });
                }

                weaponIdentityiCard.Initialize(weaponsSave);
            }

            GameSaveManager.SaveWeapons(weaponsSave);
        }

        public void ChooseWeapon(WeaponIdentityiCard weaponIdentityiCard)
        {
            var weaponsSave = GameSaveManager.GetWeaponsSave();
            weaponsSave.currentWeaponIndex = weapons.IndexOf(weaponIdentityiCard);

            GameSaveManager.SaveWeapons(weaponsSave);
        }
        public int GetCurrentWeaponIndex()
        {
            var weaponsSave = GameSaveManager.GetWeaponsSave();

            if (weaponsSave.currentWeaponIndex >= weapons.Count)
            {
                weaponsSave.currentWeaponIndex = 0;
                GameSaveManager.SaveWeapons(weaponsSave);
            }

            var currentWeaponIndex = weaponsSave.currentWeaponIndex;

            return currentWeaponIndex;
        }

        public WeaponIdentityiCard GetCurrentWeapon()
        {
            var currentWeapon = weapons[GetCurrentWeaponIndex()];

            return currentWeapon;
        }
    }
}