using IdentityCards;
using InGameStrings;
using Observables;
using UnityEngine;

namespace DependencyInjection
{
    public class Gameplay_DependencyInstaller : DependencyInstallerBase
    {
        [SerializeField] private DataSignal<WeaponIdentityiCard> _onChangedWeapon = new(Event_DIStrings.onChangedWeapon);

        public override void Inject()
        {
            base.Inject();

            DependencyContext.diBox.Add<DataSignal<WeaponIdentityiCard>>(_onChangedWeapon, Event_DIStrings.onChangedWeapon);
        }

        public override void Clear()
        {
            base.Clear();

            DependencyContext.diBox?.Remove<DataSignal<WeaponIdentityiCard>>();
        }
    }
}