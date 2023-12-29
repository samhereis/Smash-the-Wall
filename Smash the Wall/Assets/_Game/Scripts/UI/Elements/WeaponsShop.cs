using DependencyInjection;
using DG.Tweening;
using IdentityCards;
using Interfaces;
using Sirenix.OdinInspector;
using SO.Lists;
using System;
using UnityEngine;

namespace UI.Elements
{
    public sealed class WeaponsShop : MonoBehaviour, IDIDependent, IInitializable, IDisposable
    {
        [Inject][SerializeField] private ListOfAllWeapons _listOfAllWeapons;

        [Header("Prefabs")]
        [Required]
        [SerializeField] private ShopWeaponUnit _shopWeaponUnitsPrefab;

        [Header("Componenets")]
        [Required]
        [SerializeField] private Transform _shopWeaponUnitsParent;

        private void OnDestroy()
        {
            Dispose();
        }

        public void Initialize()
        {
            DependencyInjector.InjectDependencies(this);

            Dispose();

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
            shopWeaponUnit.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        public void Dispose()
        {
            foreach (ShopWeaponUnit child in GetComponentsInChildren<ShopWeaponUnit>(true))
            {
                child.transform.DOKill();

                Destroy(child.gameObject);
            }
        }
    }
}