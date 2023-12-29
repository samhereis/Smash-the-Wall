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
using Weapons;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllWeapons", menuName = "SO/Lists/ListOfAllWeapons")]
    public class ListOfAllWeapons : ConfigBase, IDIDependent, ISelfValidator
    {
        [Required]
        [field: SerializeField] public List<WeaponIdentityiCard> weapons { get; private set; } = new List<WeaponIdentityiCard>();

        [Header("Debug")]
        [Inject][SerializeField] private GameSaveManager _gameSaveManager;
        [SerializeField] private Weapons_DTO _weaponSave = new Weapons_DTO();

        public virtual void Validate(SelfValidationResult result)
        {
            foreach (var weapon in weapons)
            {
                if (weapon.target == null)
                {
                    result.AddError("Weapon Identifier at index" + weapons.IndexOf(weapon) + "is broken");
                }
                else
                {
                    if (weapon.targetName == string.Empty)
                    {
                        weapon.Validate();
                    }
                }
            }
        }

        public override void Initialize()
        {
            DependencyInjector.InjectDependencies(this);

            _weaponSave = _gameSaveManager.GetWeaponsSave();

            foreach (var weaponsIdentifier in weapons)
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
                    isUnlocked = false
                };

                weaponsSave.allWeapons.Add(aWeapon_DTO);
            }

            weaponIdentityiCard.Initialize(aWeapon_DTO);
        }

        public async Task<bool> HasWeaponToUnlock()
        {
            bool hasWeaponToUnlock = false;
            var levelSave = _gameSaveManager.GetLevelSave();

            foreach (var weapon in weapons)
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
            weaponsSave.currentWeaponIndex = weapons.IndexOf(weaponIdentityiCard);
        }

        public int GetCurrentWeaponIndex()
        {
            var weaponsSave = _gameSaveManager.GetWeaponsSave();

            if (weaponsSave.currentWeaponIndex >= weapons.Count)
            {
                weaponsSave.currentWeaponIndex = 0;
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