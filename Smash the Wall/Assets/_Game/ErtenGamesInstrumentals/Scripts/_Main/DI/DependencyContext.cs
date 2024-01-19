using Configs;
using Interfaces;
using Loggers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DependencyInjection
{
    public class DependencyContext : MonoBehaviour, ISelfValidator, IInitializable
    {
        [FoldoutGroup("Debug"), ShowInInspector, ReadOnly] public static bool isGloballyInjected { get; private set; } = false;

        [SerializeField] private bool _isGlobal = false;
        [SerializeField] private bool _autoΙnitialize = false;

        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<Component>> _objects = new();
        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<ConfigBase>> _configs = new();
        [FoldoutGroup("Objects To DI"), SerializeField] private List<Dependency_DTO<ScriptableObject>> _scriptableObjects = new();

        [Space(10)]
        [SerializeField] private LoggerBase _diLogger;
        [Space(10)]

        [FoldoutGroup("Debug"), SerializeField] private DependencyInstallerBase[] _dependencyInstallers;
        [FoldoutGroup("Debug"), SerializeField, ReadOnly] public bool isInjected { get; private set; } = false;
        [FoldoutGroup("Debug"), ShowInInspector, ReadOnly] public static DIBox diBox { get; private set; }

        private void Awake()
        {
            if (_autoΙnitialize == true) { Initialize(); }
        }

        private void OnDestroy()
        {
            Clear();

#if UNITY_EDITOR

            if (EditorApplication.isPlayingOrWillChangePlaymode == false && EditorApplication.isPlaying)
            {
                Debug.Log("Exiting playmode.");
            }

#endif
        }

        public void Initialize()
        {
            _dependencyInstallers = GetComponents<DependencyInstallerBase>();

            if (diBox == null) { diBox = new DIBox(_diLogger); }

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
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            InitAll();

            isInjected = true;
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

            foreach (var hcdi in _dependencyInstallers)
            {
                hcdi.Clear();
            }

            isInjected = false;

            if (_isGlobal == true)
            {
                isGloballyInjected = false;
            }

            _diLogger?.Log("Di Cleared: ", gameObject);
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
        }

        public static void InjectDependencies(INeedDependencyInjection dIDependent)
        {
            diBox.InjectDataTo(dIDependent);
        }
    }
}