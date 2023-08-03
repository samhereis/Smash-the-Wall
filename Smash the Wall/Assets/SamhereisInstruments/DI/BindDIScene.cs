using Configs;
using Events;
using InGameStrings;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace DI
{
    public class BindDIScene : MonoBehaviour
    {
        public static bool isInjected { get; private set; } = false;

        [SerializeField] private List<MonoBehaviourToDI> _objects = new List<MonoBehaviourToDI>();
        [SerializeField] private List<ConfigToDI> _configs = new List<ConfigToDI>();
        [SerializeField] private List<SOToDI> _scriptableObjects = new List<SOToDI>();
        [SerializeField] private List<EventToDI> _eventsWithNoParameters = new List<EventToDI>();

        private void Awake()
        {
            Application.targetFrameRate = 60;

            if (isInjected == true)
            {
                Destroy(gameObject);
                return;
            }

            DIBox.Clear();

            foreach (var objectToInject in _objects)
            {
                DIBox.Add(objectToInject.Instance, objectToInject.id);
            }

            foreach (var config in _configs)
            {
                config.Instance.Initialize();
                DIBox.Add(config.Instance, config.id);
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (scriptableObject is IInitializable)
                {
                    (scriptableObject as IInitializable).Initialize();
                }

                DIBox.Add(scriptableObject.Instance, scriptableObject.id);
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                eventWithNoParameter.Initialize();
                DIBox.Add(eventWithNoParameter.Instance, eventWithNoParameter.id);
            }

            InjectEventsWithParameters();
            InjecValueEvents();

            isInjected = true;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            isInjected = false;
        }

        private void InjectEventsWithParameters()
        {

        }

        private void InjecValueEvents()
        {

        }

        [System.Serializable]
        public class MonoBehaviourToDI
        {
            public string id = "";
            public MonoBehaviour Instance;
        }

        [System.Serializable]
        public class ConfigToDI
        {
            public string id = "";
            public ConfigBase Instance;
        }

        [System.Serializable]
        public class SOToDI
        {
            public string id = "";
            public ScriptableObject Instance;
        }

        [System.Serializable]
        public class EventToDI : IInitializable
        {
            public string id = "";
            public EventWithNoParameters Instance;

            public void Initialize()
            {
                Instance = new EventWithNoParameters(id);
            }
        }
    }
}