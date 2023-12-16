using DG.Tweening;
using DI;
using Events;
using Helpers;
using IdentityCards;
using InGameStrings;
using Interfaces;
using PlayerInputHolder;
using SO.Lists;
using UnityEngine;
using Weapons;

namespace Spawners
{
    public class GunSpawner : MonoBehaviour, IDIDependent, ISubscribesToEvents
    {
        [Header("Prefabs")]
        [DI(DIStrings.inputHolder)][SerializeField] private Input_SO _input;
        [DI(DIStrings.listOfAllWeapons)][SerializeField] private ListOfAllWeapons _listOfAllWeapons;
        [DI(DIStrings.onChangedWeapon)][SerializeField] private EventWithOneParameters<WeaponIdentityiCard> _onChangedWeapon;

        [Header("Components")]
        [SerializeField] private Transform _parent;

        [Header("Debug")]
        [SerializeField] private WeaponBase _currentWeapon;

        private void Start()
        {
            (this as IDIDependent).LoadDependencies();

            ChangeWeapon(_listOfAllWeapons.GetCurrentWeapon());
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void SubscribeToEvents()
        {
            _onChangedWeapon.AddListener(ChangeWeapon);
        }

        public void UnsubscribeFromEvents()
        {
            _onChangedWeapon.RemoveListener(ChangeWeapon);
        }

        private async void ChangeWeapon(WeaponIdentityiCard weapon)
        {
            if (_currentWeapon != null) { DeleteWeapon(_currentWeapon); }

            foreach (var weaponBase in _parent.GetComponentsInChildren<WeaponBase>(true))
            {
                DeleteWeapon(weaponBase);
            }

            await AsyncHelper.Delay(0.25f);

            _currentWeapon = Instantiate(weapon.target, _parent);
            _currentWeapon.Initialize();
            _currentWeapon.transform.localScale = Vector3.zero;
            _currentWeapon.transform.localPosition = Vector3.zero;

            _currentWeapon.transform.DOScale(1, 0.25f).OnComplete(() =>
            {
                _currentWeapon.EnableInput();
            });


        }

        private void DeleteWeapon(WeaponBase weapon)
        {
            weapon.DisableInput();
            weapon.transform.DOScale(0, 0.25f).OnComplete(() =>
            {
                Destroy(weapon.gameObject);
            });
        }
    }
}