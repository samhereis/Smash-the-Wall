#if InputSystemInstalled

using DependencyInjection;
using Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Interaction
{
    public class BackButton : MonoBehaviour, IPointerClickHandler, IDIDependent
    {
        [ShowInInspector] public UnityEvent onBack { get; private set; } = new UnityEvent();

        [Inject]
        [SerializeField] private InputsService _inputContainer;

        private void Awake()
        {
            DependencyInjector.InjectDependencies(this);
        }

        public void SubscribeToEvents()
        {
            if (_inputContainer != null) { _inputContainer.onUIBackPressed += OnBack; }
        }

        public void UnsubscribeFromEvents()
        {
            if (_inputContainer != null) { _inputContainer.onUIBackPressed -= OnBack; }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnBack();
        }

        public void OnBack()
        {
            onBack?.Invoke();
        }
    }
}

#endif