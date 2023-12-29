using Configs;
using Events;
using Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DependencyInjection
{
    public class DependencyInjector : MonoBehaviour
    {
        [ShowInInspector] public static bool isGloballyInjected { get; private set; } = false;
        [ShowInInspector, ReadOnly] private bool _isGlobal = false;

        [FoldoutGroup("Objects To DI"), ShowInInspector] private Dictionary<string, Component> _objects = new();
        [FoldoutGroup("Objects To DI"), ShowInInspector] private Dictionary<string, ConfigBase> _configs = new();
        [FoldoutGroup("Objects To DI"), ShowInInspector] private Dictionary<string, ScriptableObject> _scriptableObjects = new();
        [FoldoutGroup("Objects To DI"), ShowInInspector] private Dictionary<string, EventWithNoParameters> _eventsWithNoParameters = new();

        [FoldoutGroup("Debug"), ShowInInspector] private HardCodeDependencyInjectorBase[] _hardCodeDependencyInjectors;
        [FoldoutGroup("Debug"), ShowInInspector] public bool isInjected { get; private set; } = false;

        [ShowInInspector] private ILogger _diLogger;
        [ShowInInspector] public static DIBox diBox { get; private set; }

        private void Awake()
        {
            _hardCodeDependencyInjectors = GetComponents<HardCodeDependencyInjectorBase>();
            diBox = new DIBox(_diLogger);

            if (_isGlobal == true && isGloballyInjected == true)
            {
                Destroy(gameObject);
                return;
            }

            Clear();
            Inject();
            InitAll();

            if (_isGlobal == true)
            {
                isGloballyInjected = true;
                isInjected = true;
                transform.parent = null;
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

            if (_isGlobal == false)
            {
                Clear();
            }
        }

        public static void InjectDependencies(IDIDependent dIDependent)
        {
            diBox.InjectDataTo(dIDependent);
        }

        private void Inject()
        {
            foreach (var objectToInject in _objects)
            {
                if (diBox.Get(objectToInject.Value.GetType(), objectToInject.Key, false) == null)
                {
                    diBox.Add(objectToInject.Value, objectToInject.Key);
                }
            }

            foreach (var config in _configs)
            {
                if (diBox.Get(config.Value.GetType(), config.Key, false) == null)
                {
                    diBox.Add(config.Value, config.Key);
                }
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (diBox.Get(scriptableObject.Value.GetType(), scriptableObject.Key, false) == null)
                {
                    diBox.Add(scriptableObject.Value, scriptableObject.Key);
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                if (diBox.Get(eventWithNoParameter.Value.GetType(), eventWithNoParameter.Key, false) == null)
                {
                    diBox.Add(eventWithNoParameter.Value, eventWithNoParameter.Key);
                }
            }

            foreach (var hcdi in _hardCodeDependencyInjectors)
            {
                hcdi.Inject();
            }
        }

        private void InitAll()
        {
            foreach (var objectToInject in _objects)
            {
                if (objectToInject.Value is IInitializable)
                {
                    var initializable = (objectToInject.Value as IInitializable);
                    initializable?.Initialize();
                }
            }

            foreach (var config in _configs)
            {
                config.Value.Initialize();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (scriptableObject.Value is IInitializable)
                {
                    (scriptableObject.Value as IInitializable).Initialize();
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                eventWithNoParameter.Value.Initialize(eventWithNoParameter.Key);
            }
        }

        private void Clear()
        {
            foreach (var objectToInject in _objects)
            {
                diBox.Remove(objectToInject.Value.GetType(), objectToInject.Key);
            }

            foreach (var config in _configs)
            {
                diBox.Remove(config.Value.GetType(), config.Key);
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                diBox.Remove(scriptableObject.Value.GetType(), scriptableObject.Key);
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                diBox.Remove(eventWithNoParameter.Value.GetType(), eventWithNoParameter.Key);
            }

            foreach (var hcdi in _hardCodeDependencyInjectors)
            {
                hcdi.Clear();
            }

            isInjected = false;

            if (_isGlobal == true)
            {
                isGloballyInjected = false;
            }
        }
    }
}