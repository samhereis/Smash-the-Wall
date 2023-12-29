using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayEvents
{
    public class OnTriggerEnterExit : MonoBehaviour
    {
        [ShowInInspector] public UnityEvent<Collider> ontriggerEnter = new UnityEvent<Collider>();
        [ShowInInspector] public UnityEvent<Collider> ontriggerExit = new UnityEvent<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            ontriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ontriggerExit?.Invoke(other);
        }
    }
}