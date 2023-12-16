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

        private void Awake()
        {
            (this as IDIDependent).LoadDependencies();
        }

        private async void OnEnable()
        {
            _dot.gameObject.SetActive(false);

            if (await _listOfAllWeapons.HasWeaponToUnlock())
            {
                _dot.gameObject.SetActive(true);
            }
        }
    }
}