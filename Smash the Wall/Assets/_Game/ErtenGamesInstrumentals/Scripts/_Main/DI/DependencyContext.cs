using Configs;
using Events;
using Helpers;
using Interfaces;
using Loggers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DependencyInjection
{
    public class DependencyContext : MonoBehaviour, ISelfValidator
    {
        [FoldoutGroup("Debug"), ShowInInspector, ReadOnly] public static bool isGloballyInjected { get; private set; } = false;

        [SerializeField] private bool _isGlobal = false;

        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<Component>> _objects = new();
        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<ConfigBase>> _configs = new();
        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<ScriptableObject>> _scriptableObjects = new();

        [ListDrawerSettings(ListElementLabelName = ("eventName"))]
        [FoldoutGroup("Objects To DI"), SerializeField] private List<EventWithNoParameters> _eventsWithNoParameters = new();

        [Required]
        [FoldoutGroup("Objects To DI"), SerializeField] private LoggerBase _diLogger;

        [FoldoutGroup("Debug"), SerializeField] private DependencyInstallerBase[] _dependencyInstallers;
        [FoldoutGroup("Debug"), SerializeField, ReadOnly] public bool isInjected { get; private set; } = false;
        [FoldoutGroup("Debug"), ShowInInspector, ReadOnly] public static DIBox diBox { get; private set; } = new DIBox();

        private void Awake()
        {
            _dependencyInstallers = GetComponents<DependencyInstallerBase>();
            diBox = new DIBox(_diLogger);

            if (_isGlobal == true && isGloballyInjected == true)
            {
                Destroy(gameObject);
                return;
            }

            Clear();
            Inject();

            if (_isGlobal == true)
            {
                isGloballyInjected = true;
                isInjected = true;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            InitAll();
        }

        private void OnDestroy()
        {
            Clear();

#if UNITY_EDITOR

            if (EditorApplication.isPlayingOrWillChangePlaymode == false && EditorApplication.isPlaying)
            {
                Debug.Log("Exiting playmode.");
                diBox = new();
            }

#endif
        }

        public static void InjectDependencies(INeedDependencyInjection dIDependent)
        {
            diBox.InjectDataTo(dIDependent);
        }

        private void Inject()
        {
            foreach (var objectToInject in _objects)
            {
                diBox.Add(objectToInject.dependency, id: objectToInject.id);
            }

            foreach (var config in _configs)
            {
                diBox.Add(config.dependency, id: config.id);
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                diBox.Add(scriptableObject.dependency, id: scriptableObject.id);
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                diBox.Add(eventWithNoParameter, id: eventWithNoParameter.eventName);
            }

            foreach (var hcdi in _dependencyInstallers)
            {
                hcdi.Inject();
            }
        }

        private void InitAll()
        {
            foreach (var objectToInject in _objects)
            {
                if (objectToInject is IInitializable)
                {
                    var initializable = (objectToInject.dependency as IInitializable);
                    initializable?.Initialize();
                }
            }

            foreach (var config in _configs)
            {
                config.dependency.Initialize();
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                if (scriptableObject.dependency is IInitializable)
                {
                    (scriptableObject.dependency as IInitializable).Initialize();
                }
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                eventWithNoParameter.Initialize(eventWithNoParameter.eventName);
            }
        }

        [Button]
        private void Clear()
        {
            foreach (var objectToInject in _objects)
            {
                diBox.Remove(objectToInject.dependency.GetType(), objectToInject.id);
            }

            foreach (var config in _configs)
            {
                diBox.Remove(config.dependency.GetType(), config.id);
            }

            foreach (var scriptableObject in _scriptableObjects)
            {
                diBox.Remove(scriptableObject.dependency.GetType(), scriptableObject.id);
            }

            foreach (var eventWithNoParameter in _eventsWithNoParameters)
            {
                diBox.Remove(eventWithNoParameter.GetType(), eventWithNoParameter.eventName);
            }

            foreach (var hcdi in _dependencyInstallers)
            {
                hcdi.Clear();
            }

            isInjected = false;

            if (_isGlobal == true)
            {
                isGloballyInjected = false;
            }

            Debug.Log(gameObject.name + "Di Cleared: ", gameObject);
        }

        public void Validate(SelfValidationResult result)
        {
            foreach (var scriptableObject in _scriptableObjects)
            {
                var allDuplicateTypes = _scriptableObjects.FindAll((x =>
                {
                    return x.dependency.GetType() == scriptableObject.dependency.GetType() && x.id == scriptableObject.id;
                }));

                if (allDuplicateTypes.Count > 1)
                {
                    result.AddError("There is multiple types of " + scriptableObject.dependency.GetType()).WithFix(() =>
                    {
                        foreach (var duplicate in allDuplicateTypes)
                        {
                            duplicate.id = duplicate.dependency.name;
                        }
                    });

                    break;
                }
            }

            foreach (var anEvent in _eventsWithNoParameters)
            {
                if (anEvent.eventName == string.Empty)
                {
                    result.AddError("Event in index " + _eventsWithNoParameters.IndexOf(anEvent) + " has no event name");
                }
            }

            if (_diLogger == null)
            {
                result.AddError("DI logger is null").WithFix(() =>
                {
                    _diLogger = GetComponentInChildren<LoggerBase>();
                    this.TrySetDirty();
                });
            }
        }
    }
}