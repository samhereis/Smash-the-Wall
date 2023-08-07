using DI;
using Events;
using IdentityCards;
using InGameStrings;
using Interfaces;
using Managers;
using ProjectSripts;
using SO.Lists;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public sealed class ShopWeaponUnit : MonoBehaviour, IDIDependent, IInitializable<WeaponIdentityiCard>
    {
        [Header("DI")]
        [DI(DIStrings.onChangedWeapon)][SerializeField] private EventWithOneParameters<WeaponIdentityiCard> _onChangedWeapon;
        [DI(DIStrings.listOfAllWeapons)][SerializeField] private ListOfAllWeapons _listOfAllWeapons;

        [Header("Components")]
        [SerializeField] private Image _weaponImage;
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private Button _itemButton;

        [Header("Debug")]
        [SerializeField] private WeaponIdentityiCard _weaponIdentityiCard;

        public void Initialize(WeaponIdentityiCard weaponIdentityiCard)
        {
            (this as IDIDependent).LoadDependencies();

            _weaponIdentityiCard = weaponIdentityiCard;

            _weaponName.text = weaponIdentityiCard.targetName;

            _itemButton.onClick.RemoveListener(TryOpen);
            _itemButton.onClick.RemoveListener(Choose);

            if (weaponIdentityiCard.isUnlocked == false)
            {
                _itemButton.onClick.AddListener(TryOpen);
                _weaponImage.gameObject.SetActive(true);
            }
            else
            {
                _itemButton.onClick.AddListener(Choose);
                _weaponImage.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _itemButton.onClick.RemoveListener(TryOpen);
            _itemButton.onClick.RemoveListener(Choose);
        }

        private void TryOpen()
        {
            if (_weaponIdentityiCard.isUnlocked == false)
            {
                AdsShowManager.instance.TryShowRewarded(OnOpen);
            }
            else
            {
                Initialize(_weaponIdentityiCard);
            }
        }

        private void OnOpen()
        {
            _itemButton.onClick.RemoveListener(TryOpen);

            _listOfAllWeapons.UnlockWeapon(_weaponIdentityiCard);
            Choose();

            Initialize(_weaponIdentityiCard);
        }

        private void Choose()
        {
            _onChangedWeapon?.Invoke(_weaponIdentityiCard);
            _listOfAllWeapons.ChooseWeapon(_weaponIdentityiCard);
        }
    }
}