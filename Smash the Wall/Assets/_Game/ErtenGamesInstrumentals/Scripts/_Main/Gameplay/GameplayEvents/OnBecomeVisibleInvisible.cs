using UnityEngine;
using UnityEngine.Events;

namespace GameplayEvents
{
    public class OnBecomeVisibleInvisible : MonoBehaviour
    {
        [SerializeField] public bool isVisible { get; private set; } = true;

        [SerializeField] public UnityEvent onBecomeVisible { get; private set; }
        [SerializeField] public UnityEvent onBecomeInvisible { get; private set; }

        [SerializeField] private MonoBehaviour[] _activateOnVisible;
        [SerializeField] private MonoBehaviour[] _deactivateOnVisible;

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