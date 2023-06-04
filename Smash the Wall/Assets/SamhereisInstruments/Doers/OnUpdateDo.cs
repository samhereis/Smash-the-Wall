using UnityEngine;
using UnityEngine.Events;

namespace Helpers.OnUnityEventDoers
{
    public class OnUpdateDo : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onUpdateDo;

        private void Update()
        {
            _onUpdateDo?.Invoke();
        }
    }
}