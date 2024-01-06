using DependencyInjection;
using Sirenix.OdinInspector;
using SO.Lists;
using UnityEngine;

namespace UI.Elements
{
    public class StoreButton : MonoBehaviour, INeedDependencyInjection
    {
        [Header("DI")]
        [Inject][SerializeField] private ListOfAllWeapons _listOfAllWeapons;

        [Header("Components")]
        [Required]
        [SerializeField] private Transform _dot;

        private void Awake()
        {
            DependencyContext.InjectDependencies(this);
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