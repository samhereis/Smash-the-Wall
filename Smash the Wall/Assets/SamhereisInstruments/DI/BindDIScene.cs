using Configs;
using Events;
using IdentityCards;
using InGameStrings;
using Interfaces;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DI
{
    public class BindDIScene : MonoBehaviour
    {
        public static bool isGLoballyInhected { get; private set; } = false;

        [Header("Objects To DI")]
        [SerializeField] private List<MonoBehaviourToDI> _objects = new List<MonoBehaviourToDI>();
        [SerializeField] private List<ConfigToDI> _configs = new List<ConfigToDI>();
        [SerializeField] private List<SOToDI> _scriptableObjects = new List<SOToDI>();
        [SerializeField] private List<EventToDI> _eventsWithNoParameters = new List<EventToDI>();

        [Header("Settings")]
        [SerializeField] private bool _isGlobal = false;

        private void Awake()
        {
            if (_isGlobal == true && isGLoballyInhected == true)
            {
                Destroy(gameObject);
                return;
            }

            Clear();

            foreach (var objectToInject in _objects)
            {
                if (DIBox.Get(objectToInject.instance.GetType(), objectToInject.id, false) == null)
                {
                    DIBox.Add(objectToInject.instance, objectToInject.id);
                }
            }

            foreach (var config in _configs)
            {
                if (DIBox.Get(config.instance.GetType(), config.id, false) == null)
                {
                    config.instance.Initialize();
                    DIBox.Add(config.instance, config.id);
                }
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (DIBox.Get(scriptableObject.instance.GetType(), scriptableObject.id, false) == null)
                {
                    if (scriptableObject is IInitializable)
                    {
                        (scriptableObject as IInitializable).Initialize();
                    }

                    DIBox.Add(scriptableObject.instance, scriptableObject.id);
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                if (DIBox.Get(eventWithNoParameter.instance.GetType(), eventWithNoParameter.id, false) == null)
                {
                    eventWithNoParameter.Initialize();
                    DIBox.Add(eventWithNoParameter.instance, eventWithNoParameter.id);
                }
            }

            InjectEventsWithParameters();
            InjecValueEvents();

            if (_isGlobal == true)
            {
                isGLoballyInhected = true;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {

#if UNITY_EDITOR

            if (EditorApplication.isPlayingOrWillChangePlaymode == false && EditorApplication.isPlaying)
            {
                Debug.Log("Exiting playmode.");
                Clear();
            }
#endif

            if (_isGlobal == true && isGLoballyInhected == true)
            {
                return;
            }

            Clear();
        }

        private void InjectEventsWithParameters()
        {
            if (DIBox.Get<EventWithOneParameters<WeaponIdentityiCard>>(DIStrings.onChangedWeapon, false) == null)
            {
                DIBox.Add(new EventWithOneParameters<WeaponIdentityiCard>(DIStrings.onChangedWeapon), DIStrings.onChangedWeapon);
            }
        }

        private void InjecValueEvents()
        {

        }

        private void ClearEventsWithParameters()
        {
            DIBox.Remove<EventWithOneParameters<WeaponIdentityiCard>>(DIStrings.onChangedWeapon);
        }

        private void ClearValueEvents()
        {

        }

        private void Clear()
        {
            foreach (var objectToInject in _objects)
            {
                DIBox.Remove(objectToInject.instance.GetType(), objectToInject.id);
            }

            foreach (var config in _configs)
            {
                DIBox.Remove(config.instance.GetType(), config.id);
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                DIBox.Remove(scriptableObject.instance.GetType(), scriptableObject.id);
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                DIBox.Remove(eventWithNoParameter.instance.GetType(), eventWithNoParameter.id);
            }

            ClearEventsWithParameters();
            ClearValueEvents();

            if (_isGlobal == true)
            {
                isGLoballyInhected = false;
            }
        }

        [Serializable]
        public class MonoBehaviourToDI
        {
            public string id = "";
            public MonoBehaviour instance;
        }

        [Serializable]
        public class ConfigToDI
        {
            public string id = "";
            public ConfigBase instance;
        }

        [Serializable]
        public class SOToDI
        {
            public string id = "";
            public ScriptableObject instance;
        }

        [Serializable]
        public class EventToDI : IInitializable
        {
            public string id = "";
            public EventWithNoParameters instance;

            public void Initialize()
            {
                instance = new EventWithNoParameters(id);
            }
        }
    }
}