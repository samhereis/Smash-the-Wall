using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayEvents
{
    public class OnBecomeVisibleInvisible : MonoBehaviour
    {
        [ShowInInspector] public bool isVisible { get; private set; } = true;

        [ShowInInspector] public UnityEvent onBecomeVisible { get; private set; }
        [ShowInInspector] public UnityEvent onBecomeInvisible { get; private set; }

        [ShowInInspector] private MonoBehaviour[] _activateOnVisible;
        [ShowInInspector] private MonoBehaviour[] _deactivateOnVisible;

        private void OnBecameVisible()
        {
            SetVisible(true);

            onBecomeVisible?.Invoke();
        }

        private void OnBecameInvisible()
        {
            SetVisible(false);

            onBecomeInvisible?.Invoke();
        }

        private void SetVisible(bool visible)
        {
            foreach (var monobeh in _activateOnVisible)
            {
                monobeh.enabled = visible == true;
            }

            foreach (var monobeh in _deactivateOnVisible)
            {
                monobeh.enabled = visible == false;
            }

            isVisible = visible;
        }
    }
}