using Configs;
using DependencyInjection;
using DTO;
using DTO.Save;
using Helpers;
using IdentityCards;
using Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllWeapons", menuName = "Scriptables/Lists/ListOfAllWeapons")]
    public class ListOfAllWeapons : ConfigBase, INeedDependencyInjection
    {
        public IEnumerable<WeaponIdentityiCard> weapons => _weapons;

        [Required]
        [ListDrawerSettings(ListElementLabelName = ("targetName"))]
        [SerializeField] private List<WeaponIdentityiCard> _weapons = new List<WeaponIdentityiCard>();

        [Header("Debug")]
        [SerializeField, ReadOnly] private Weapons_DTO _weaponSave = new Weapons_DTO();

        [Inject] private GameSaveManager _gameSaveManager;

        [Button]
        public virtual void Validate()
        {
            foreach (var weapon in _weapons)
            {
                weapon.Setup();
            }
        }

        public override void Initialize()
        {
            DependencyContext.InjectDependencies(this);

            _weaponSave = _gameSaveManager.GetWeaponsSave();

            foreach (var weaponsIdentifier in _weapons)
            {
                weaponsIdentifier.SetIsUnlockedStatus(false);

                InitWeapon(_weaponSave, weaponsIdentifier);
            }
        }

        private void InitWeapon(Weapons_DTO weaponsSave, WeaponIdentityiCard weaponIdentityiCard)
        {
            var aWeapon_DTO = weaponsSave.allWeapons.Find(x => x.weaponName == weaponIdentityiCard.targetName);

            if (aWeapon_DTO == null)
            {
                aWeapon_DTO = new AWeapon_DTO
                {
                    weaponName = weaponIdentityiCard.targetName,
                    isUnlocked = weaponIdentityiCard.isDefault ? true : aWeapon_DTO.isUnlocked
                };

                weaponsSave.allWeapons.Add(aWeapon_DTO);
            }
        }

        public async Task<bool> HasWeaponToUnlock()
        {
            bool hasWeaponToUnlock = false;
            var levelSave = _gameSaveManager.GetLevelSave();

            foreach (var weapon in _weapons)
            {
                await AsyncHelper.Skip();

                if (weapon.IsToUnlock(levelSave))
                {
                    hasWeaponToUnlock = true;
                    break;
                }
            }

            return hasWeaponToUnlock;
        }

        public void ChooseWeapon(WeaponIdentityiCard weaponIdentityiCard)
        {
            var weaponsSave = _gameSaveManager.GetWeaponsSave();
            weaponsSave.currentWeaponIndex = _weapons.IndexOf(weaponIdentityiCard);

            _gameSaveManager.SaveWeapons();
        }

        public int GetCurrentWeaponIndex()
        {
            var weaponsSave = _gameSaveManager.GetWeaponsSave();

            if (weaponsSave.currentWeaponIndex >= _weapons.Count)
            {
                weaponsSave.currentWeaponIndex = 0;
            }

            var currentWeaponIndex = weaponsSave.currentWeaponIndex;

            return currentWeaponIndex;
        }

        public WeaponIdentityiCard GetCurrentWeapon()
        {
            var currentWeapon = _weapons[GetCurrentWeaponIndex()];

            return currentWeapon;
        }

        public void UnlockWeapon(WeaponIdentityiCard weaponIdentityiCard)
        {
            var weaponsSave = _gameSaveManager.GetWeaponsSave();

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

            Initialize();
        }
    }
}