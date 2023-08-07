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

        public override void Initialize()
        {
            var weaponsSave = GameSaveManager.GetWeaponsSave();

            foreach (var weaponsIdentifier in weapons)
            {
                InitWeapon(weaponsIdentifier);
            }

            void InitWeapon(WeaponIdentityiCard weaponIdentityiCard)
            {
                var aWeapon_DTO = weaponsSave.allWeapons.Find(x => x.weaponName == weaponIdentityiCard.targetName);

                if (aWeapon_DTO == null)
                {
                    var newAWeapon_STO = new AWeapon_DTO
                    {
                        weaponName = weaponIdentityiCard.targetName,
                        isUnlocked = false
                    };

                    if (weaponIdentityiCard.isDefault == true)
                    {
                        newAWeapon_STO.isUnlocked = true;
                    }

                    weaponsSave.allWeapons.Add(newAWeapon_STO);
                }

                aWeapon_DTO = weaponsSave.allWeapons.Find(x => x.weaponName == weaponIdentityiCard.targetName);

                weaponIdentityiCard.Initialize(aWeapon_DTO);
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

        public void UnlockWeapon(WeaponIdentityiCard weaponIdentityiCard)
        {
            var weaponsSave = GameSaveManager.GetWeaponsSave();

            var weaponSaveUnit = weaponsSave.allWeapons.Find(x => x.weaponName == weaponIdentityiCard.targetName);

            if (weaponSaveUnit != null)
            {
                weaponSaveUnit.isUnlocked = true;
            }
            else
            {
                weaponsSave.allWeapons.Add(new AWeapon_DTO()
                {
                    weaponName = weaponIdentityiCard.targetName,
                    isUnlocked = true
                });
            }

            GameSaveManager.SaveWeapons(weaponsSave);

            Initialize();
        }
    }
}