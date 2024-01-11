using DependencyInjection;
using DG.Tweening;
using Helpers;
using IdentityCards;
using InGameStrings;
using Interfaces;
using Managers;
using Observables;
using Services;
using Sirenix.OdinInspector;
using SO.Lists;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public sealed class ShopWeaponUnit : MonoBehaviour, INeedDependencyInjection, IInitializable<WeaponIdentityiCard>
    {
        [Header("Components")]
        [Required]
        [SerializeField] private Image _weaponImage;

        [Required]
        [SerializeField] private Image _tapToGetImage;

        [Required]
        [SerializeField] private Image _lockImage;

        [Space(10)]
        [Required]
        [SerializeField] private TextMeshProUGUI _label;

        [Space(10)]
        [Required]
        [SerializeField] private Button _itemButton;

        [Space(10)]
        [Required]
        [SerializeField] private Transform _holder;

        [Required]
        [SerializeField] private Transform _getHolder;

        [Header("Settings")]
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private float _shakeStrenth = 30f;
        [SerializeField] private int _shakeVibro = 30;

        private WeaponIdentityiCard _weaponIdentityiCard;

        [Inject(Event_DIStrings.onChangedWeapon)] private DataSignal<WeaponIdentityiCard> _onChangedWeapon;
        [Inject] private ListOfAllWeapons _listOfAllWeapons;
        [Inject] private AdsShowManager _adsShowManager;
        [Inject] private GameSaveManager _gameSaveManager;

        private bool _canShoose = true;

        private void Awake()
        {
            DependencyContext.InjectDependencies(this);
        }

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

            await AsyncHelper.DelayFloat(1f);
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