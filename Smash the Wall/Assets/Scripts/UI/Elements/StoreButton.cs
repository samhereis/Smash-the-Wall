using DG.Tweening;
using DI;
using InGameStrings;
using SO.Lists;
using UnityEngine;

namespace UI.Elements
{
    public class StoreButton : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.listOfAllWeapons)][SerializeField] private ListOfAllWeapons _listOfAllWeapons;

        [Header("Components")]
        [SerializeField] private Transform _dot;

        [Header("Settings")]
        [SerializeField] private float _upScale;
        [SerializeField] private float _duration;

        private void Awake()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private async void OnEnable()
        {
            if (await _listOfAllWeapons.HasWeaponToUnlock())
            {
                _dot.DOScale(_upScale, _duration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _dot.transform.localScale = Vector3.zero;
            }
        }

        private void OnDisable()
        {
            _dot.DOKill();
        }
    }
}