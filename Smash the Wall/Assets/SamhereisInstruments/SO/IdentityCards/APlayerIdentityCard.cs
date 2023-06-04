using Identifiers;
using UnityEngine;

namespace IdentityCards
{
    [CreateAssetMenu(fileName = "APlayerIdentityCard ", menuName = "Scriptables/IdentityCards/APlayerIdentityCard")]
    public sealed class APlayerIdentityCard : IdentityCardBase<IdentifierBase>
    {
        [field: SerializeField] public IdentifierBase model;

        private void OnValidate()
        {

        }
    }
}