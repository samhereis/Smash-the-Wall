using Configs;
using IdentityCards;
using Managers;
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

            defaultWeapon.SetIsUnlockedStatus(true);

            foreach (var weaponsIdentifier in weapons)
            {
                weaponsIdentifier.Initialize(weaponsSave);
            }
        }
    }
}