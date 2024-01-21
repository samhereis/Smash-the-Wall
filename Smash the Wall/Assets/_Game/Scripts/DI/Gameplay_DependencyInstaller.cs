using IdentityCards;
using InGameStrings;
using Observables;
using UnityEngine;
using static GameState.Gameplay_GameState_Model;

namespace DependencyInjection
{
    public class Gameplay_DependencyInstaller : DependencyInstallerBase
    {
        [SerializeField] private DataSignal<GameplayState> onGameplayStatucChanged = new(Event_DIStrings.onGameplayStatucChanged);
        [SerializeField] private DataSignal<WeaponIdentityiCard> _onChangedWeapon = new(Event_DIStrings.onChangedWeapon);

        public override void Inject()
        {
            base.Inject();

            DependencyContext.diBox.Add<DataSignal<GameplayState>>(onGameplayStatucChanged, Event_DIStrings.onGameplayStatucChanged);
            DependencyContext.diBox.Add<DataSignal<WeaponIdentityiCard>>(_onChangedWeapon, Event_DIStrings.onChangedWeapon);
        }

        public override void Clear()
        {
            base.Clear();

            DependencyContext.diBox?.Remove<DataSignal<GameplayState>>(Event_DIStrings.onGameplayStatucChanged);
            DependencyContext.diBox?.Remove<DataSignal<WeaponIdentityiCard>>(Event_DIStrings.onChangedWeapon);
        }
    }
}