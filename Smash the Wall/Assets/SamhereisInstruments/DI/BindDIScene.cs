using InGameStrings;
using Configs;
using Events;
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

        private void Awake()
        {
            Application.targetFrameRate = 60;

            if (isInjected == true)
            {
                Destroy(gameObject);
                return;
            }

            DIBox.Clear();

            foreach (var obj in _objects)
            {
                DIBox.Add(obj.Instance, obj.id);
            }

            foreach (var obj in _configs)
            {
                obj.Instance.Initialize();
                DIBox.Add(obj.Instance, obj.id);
            }

            foreach (var obj in _scriptableObjects)
            {
                DIBox.Add(obj.Instance, obj.id);
            }

            InjectEventWithNoEvents();
            InjectEventsWithParameters();
            InjecValueEvents();

            isInjected = true;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            isInjected = false;
        }

        private void InjectEventWithNoEvents()
        {
            DIBox.Add<EventWithNoParameters>(new EventWithNoParameters(DIStrings.onWinEvent), DIStrings.onWinEvent);
            DIBox.Add<EventWithNoParameters>(new EventWithNoParameters(DIStrings.onLoseEvent), DIStrings.onLoseEvent);
            DIBox.Add<EventWithNoParameters>(new EventWithNoParameters(DIStrings.onNoAdsBought), DIStrings.onNoAdsBought);
            DIBox.Add<EventWithNoParameters>(new EventWithNoParameters(DIStrings.onGameSceneLoad), DIStrings.onGameSceneLoad);
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
    }
}