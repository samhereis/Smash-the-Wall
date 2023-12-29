using DependencyInjection;
using Displayers;
using Interfaces;
using Services;
using Sirenix.OdinInspector;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IInitializable, IHasInput
    {
        [SerializeField] protected SoundPlayer _soundPlayer => SoundPlayer.instance;

        [Header("DI")]
        [Inject][SerializeField] protected InputsService _input;
        [Inject][SerializeField] protected TrajectoryDisplayer _trajectoryDisplayer;

        [Header("Components")]
        [Required]
        [SerializeField] protected Transform _gunPoint;

        [field: SerializeField] public Transform shootPosition { get; protected set; }

        [field: SerializeField, Header("Current State")] public bool canShoot { get; protected set; }

        private void OnDisable()
        {
            _trajectoryDisplayer.HideTrajectory();
        }

        public virtual void Initialize()
        {

        }

        public abstract void EnableInput();
        public abstract void DisableInput();
        protected abstract void Fire(InputAction.CallbackContext context);
        public abstract void OnFired();


        public override string ToString()
        {
            return gameObject.name;
        }
    }
}