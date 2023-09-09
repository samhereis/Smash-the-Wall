using Configs;
using Events;
using Helpers;
using IdentityCards;
using InGameStrings;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DI
{
    public class BindDIScene : MonoBehaviour
    {
        public static bool isGLoballyInjected { get; private set; } = false;

        [Header("Objects To DI")]
        [SerializeField] private List<MonoBehaviourToDI> _objects = new List<MonoBehaviourToDI>();
        [SerializeField] private List<ConfigToDI> _configs = new List<ConfigToDI>();
        [SerializeField] private List<SOToDI> _scriptableObjects = new List<SOToDI>();
        [SerializeField] private List<EventToDI> _eventsWithNoParameters = new List<EventToDI>();

        [Header("Settings")]
        [SerializeField] private bool _isGlobal = false;

        private async void Awake()
        {
            if (_isGlobal == true && isGLoballyInjected == true)
            {
                Destroy(gameObject);
                return;
            }

            if (_isGlobal == false)
            {
                Clear();

                Inject();
                InjectEventsWithParameters();
                InjecValueEvents();

                InitAll();
            }
            else
            {
                await Task.Run(async () =>
                {
                    await ClearAsync();

                    await InjectAsync();
                    InjectEventsWithParameters();
                    InjecValueEvents();
                });

                await InitAllAsync();
            }

            if (_isGlobal == true)
            {
                isGLoballyInjected = true;
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

            if (_isGlobal == true && isGLoballyInjected == true)
            {
                return;
            }

            Clear();
        }

        private void Inject()
        {
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
                    DIBox.Add(config.instance, config.id);
                }
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (DIBox.Get(scriptableObject.instance.GetType(), scriptableObject.id, false) == null)
                {
                    DIBox.Add(scriptableObject.instance, scriptableObject.id);
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                if (DIBox.Get(eventWithNoParameter.instance.GetType(), eventWithNoParameter.id, false) == null)
                {
                    DIBox.Add(eventWithNoParameter.instance, eventWithNoParameter.id);
                }
            }
        }

        private async Task InjectAsync()
        {
            foreach (var objectToInject in _objects)
            {
                DIBox.Add(objectToInject.instance, objectToInject.id);
                await AsyncHelper.Delay();
            }

            foreach (var config in _configs)
            {
                DIBox.Add(config.instance, config.id);
                await AsyncHelper.Delay();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                DIBox.Add(scriptableObject.instance, scriptableObject.id);
                await AsyncHelper.Delay();
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                DIBox.Add(eventWithNoParameter.instance, eventWithNoParameter.id);
                await AsyncHelper.Delay();
            }
        }

        private void InitAll()
        {
            foreach (var objectToInject in _objects)
            {
                if (objectToInject.instance is IInitializable)
                {
                    (objectToInject.instance as IInitializable).Initialize();
                }
            }

            foreach (var config in _configs)
            {
                config.instance.Initialize();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (scriptableObject.instance is IInitializable)
                {
                    (scriptableObject.instance as IInitializable).Initialize();
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                eventWithNoParameter.Initialize();
            }
        }

        private async Task InitAllAsync()
        {
            foreach (var objectToInject in _objects)
            {
                (objectToInject.instance as IInitializable)?.Initialize();
                await AsyncHelper.Delay();
            }

            foreach (var config in _configs)
            {
                config.instance.Initialize();
                await AsyncHelper.Delay();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                (scriptableObject.instance as IInitializable)?.Initialize();
                await AsyncHelper.Delay();
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                eventWithNoParameter.Initialize();
                await AsyncHelper.Delay();
            }
        }

        private void InjectEventsWithParameters()
        {
            DIBox.Add(new EventWithOneParameters<WeaponIdentityiCard>(DIStrings.onChangedWeapon), DIStrings.onChangedWeapon);
        }

        private void InjecValueEvents()
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
                isGLoballyInjected = false;
            }
        }

        private async Task ClearAsync()
        {
            foreach (var objectToInject in _objects)
            {
                DIBox.Remove(objectToInject.instance.GetType(), objectToInject.id);
                await AsyncHelper.Delay();
            }

            foreach (var config in _configs)
            {
                DIBox.Remove(config.instance.GetType(), config.id);
                await AsyncHelper.Delay();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                DIBox.Remove(scriptableObject.instance.GetType(), scriptableObject.id);
                await AsyncHelper.Delay();
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                DIBox.Remove(eventWithNoParameter.instance.GetType(), eventWithNoParameter.id);
                await AsyncHelper.Delay();
            }

            ClearEventsWithParameters();
            ClearValueEvents();

            if (_isGlobal == true)
            {
                isGLoballyInjected = false;
            }
        }

        private void ClearEventsWithParameters()
        {
            DIBox.Remove<EventWithOneParameters<WeaponIdentityiCard>>(DIStrings.onChangedWeapon);
        }

        private void ClearValueEvents()
        {

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