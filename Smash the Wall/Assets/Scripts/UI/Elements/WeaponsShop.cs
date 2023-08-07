using DG.Tweening;
using DI;
using IdentityCards;
using InGameStrings;
using Interfaces;
using SO.Lists;
using UnityEngine;

namespace UI.Elements
{
    public sealed class WeaponsShop : MonoBehaviour, IDIDependent, IInitializable, IClearable
    {
        [DI(DIStrings.listOfAllWeapons)][SerializeField] private ListOfAllWeapons _listOfAllWeapons;

        [Header("Prefabs")]
        [SerializeField] private ShopWeaponUnit _shopWeaponUnitsPrefab;

        [Header("Componenets")]
        [SerializeField] private Transform _shopWeaponUnitsParent;
        private void Start()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private void OnDestroy()
        {
            Clear();
        }

        public void Initialize()
        {
            Clear();

            foreach (var weapon in _listOfAllWeapons.weapons)
            {
                InstantiateShopWeaponUnit(weapon);
            }

        }
        private void InstantiateShopWeaponUnit(WeaponIdentityiCard weaponIdentityiCard)
        {
            var shopWeaponUnit = Instantiate(_shopWeaponUnitsPrefab, _shopWeaponUnitsParent);
            shopWeaponUnit.transform.localScale = Vector3.zero;
            shopWeaponUnit.Initialize(weaponIdentityiCard);
            shopWeaponUnit.transform.DOScale(1, 0.5f);
        }

        public void Clear()
        {
            foreach (ShopWeaponUnit child in GetComponentsInChildren<ShopWeaponUnit>(true))
            {
                Destroy(child.gameObject);
            }
        }
    }
}