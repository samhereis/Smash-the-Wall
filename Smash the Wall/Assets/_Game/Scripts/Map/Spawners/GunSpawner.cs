using DependencyInjection;
using DG.Tweening;
using Events;
using Helpers;
using IdentityCards;
using Interfaces;
using Services;
using Sirenix.OdinInspector;
using SO.Lists;
using UnityEngine;
using Weapons;

namespace Spawners
{
    public class GunSpawner : MonoBehaviour, INeedDependencyInjection, ISubscribesToEvents
    {
        [Header("Prefabs")]
        [Inject][SerializeField] private InputsService _input;
        [Inject][SerializeField] private ListOfAllWeapons _listOfAllWeapons;
        [Inject][SerializeField] private EventWithOneParameters<WeaponIdentityiCard> _onChangedWeapon;

        [Header("Components")]
        [Required]
        [SerializeField] private Transform _parent;

        [Header("Debug")]
        [SerializeField] private WeaponBase _currentWeapon;

        private void Start()
        {
            DependencyContext.InjectDependencies(this);

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

            await AsyncHelper.DelayFloat(0.25f);

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