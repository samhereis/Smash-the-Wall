using DG.Tweening;
using DI;
using Events;
using Helpers;
using IdentityCards;
using InGameStrings;
using Interfaces;
using Managers;
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
        [DI(DIStrings.adsShowManager)][SerializeField] private AdsShowManager _adsShowManager;
        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;

        [Header("Components")]
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Image _tapToGetImage;
        [SerializeField] private Image _lockImage;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _label;

        [Space(10)]
        [SerializeField] private Button _itemButton;

        [Space(10)]
        [SerializeField] private Transform _holder;
        [SerializeField] private Transform _getHolder;

        [Header("Settings")]
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private float _shakeStrenth = 30f;
        [SerializeField] private int _shakeVibro = 30;

        [Header("Debug")]
        [SerializeField] private WeaponIdentityiCard _weaponIdentityiCard;

        private bool _canShoose = true;

        private void OnDisable()
        {
            _itemButton.onClick.RemoveListener(TryOpen);
            _itemButton.onClick.RemoveListener(Choose);
            _itemButton.onClick.RemoveListener(OnClickWhileUnavailable);

            _tapToGetImage.DOKill();
            _tapToGetImage.transform.DOKill();
            _holder.DOKill();
        }

        public void Initialize(WeaponIdentityiCard weaponIdentityiCard)
        {
            (this as IDIDependent).LoadDependencies();

            _weaponIdentityiCard = weaponIdentityiCard;

            _label.text = weaponIdentityiCard.targetName;

            _itemButton.onClick.RemoveListener(TryOpen);
            _itemButton.onClick.RemoveListener(Choose);
            _itemButton.onClick.RemoveListener(OnClickWhileUnavailable);
            _lockImage.gameObject.SetActive(false);
            _tapToGetImage.gameObject.SetActive(false);
            _getHolder.gameObject.SetActive(false);

            var levelSave = _gameSaveManager.GetLevelSave();

            if (weaponIdentityiCard.isUnlocked)
            {
                _itemButton.onClick.AddListener(Choose);
            }
            else
            {
                if (weaponIdentityiCard.IsToUnlock(levelSave))
                {
                    _getHolder.gameObject.SetActive(true);
                    _tapToGetImage.gameObject.SetActive(true);
                    _itemButton.onClick.AddListener(TryOpen);

                    _tapToGetImage.transform.DOScale(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
                }
                else
                {
                    _itemButton.onClick.AddListener(OnClickWhileUnavailable);
                    _lockImage.gameObject.SetActive(true);

                    _label.text = "opens at level: " + _weaponIdentityiCard.opensAtLevel;
                }

                _label.text = _weaponIdentityiCard.targetName;
            }

            _weaponImage.sprite = _weaponIdentityiCard.icon;
        }

        private void TryOpen()
        {
            if (_weaponIdentityiCard.isUnlocked == false)
            {
                _adsShowManager?.TryShowRewarded(OnOpen);
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

            _tapToGetImage.DOKill();
        }

        private async void Choose()
        {
            if (_canShoose == false) return;
            _canShoose = false;

            _onChangedWeapon?.Invoke(_weaponIdentityiCard);
            _listOfAllWeapons.ChooseWeapon(_weaponIdentityiCard);

            await AsyncHelper.Delay(1f);
            _canShoose = true;
        }

        private void OnClickWhileUnavailable()
        {
            _holder.DOKill();
            _holder.localPosition = Vector3.zero;
            _holder.DOShakePosition(_shakeDuration, _shakeStrenth, _shakeVibro).SetEase(Ease.InOutBack);
        }
    }
}