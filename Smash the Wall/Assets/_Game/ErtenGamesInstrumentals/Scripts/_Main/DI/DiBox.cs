using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace DependencyInjection
{
    [Serializable]
    public class DIBox
    {
        [ShowInInspector] private readonly Dictionary<Type, Dictionary<string, object>> _dictionarySingle = new();
        [ShowInInspector] private ILogger _logger;

        public DIBox(ILogger logger)
        {
            _logger = logger;
        }

        public void Clear()
        {
            _dictionarySingle.Clear();
        }

        public void Add<T>(T instance, string id = "")
        {
            if (instance == null) Debug.LogWarning($"Instance is null - type {instance.GetType()}");

            AddToDictionary(instance, id, instance.GetType());
        }

        public void Add(Object instance, string id = "")
        {
            if (instance == null) Debug.LogWarning($"Instance is null");

            Type typeInstance = instance.GetType();

            AddToDictionary(instance, id, typeInstance);
        }

        public void Remove<T>(string id = "") where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)))
            {
                _dictionarySingle[typeof(T)].Remove(id);

                _logger?.Log($"Removed from DI - Type: {typeof(T)} - Id: '{id}'");
            }
            if (_dictionarySingle.Count == 0)
            {
                _dictionarySingle.Remove(typeof(T));
            }
        }

        public void Remove(Type type, string id = "")
        {
            if (_dictionarySingle.ContainsKey(type))
            {
                _dictionarySingle[type].Remove(id);
                _logger?.Log($"Removed from DI - Type: {type} - Id: '{id}'");
            }
            if (_dictionarySingle.Count == 0)
            {
                _dictionarySingle.Remove(type);
            }
        }

        public T Get<T>(string id = "", bool logErrors = true) where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)) == false)
            {
                if (logErrors == true)
                {
                    Debug.LogWarning($"DI container does not contain this type  - Type: {typeof(T)}");
                }

                return null;
            }

            if (_dictionarySingle[typeof(T)].ContainsKey(id) == false)
            {
                if (logErrors == true)
                {
                    Debug.LogWarning($"The container does not contain under this ID - Type: {typeof(T)} \\ Id: '{id}'");
                }

                return null;
            }

            return _dictionarySingle[typeof(T)][id] as T;
        }

        public object Get(Type type, string id = "", bool logErrors = true)
        {
            if (_dictionarySingle.ContainsKey(type) == false)
            {
                if (logErrors == true)
                {
                    Debug.LogWarning($"DI container does not contain this type  - Type: {type}");
                }

                return null;
            }

            if (_dictionarySingle[type].ContainsKey(id) == false)
            {
                if (logErrors == true)
                {
                    Debug.LogWarning($"The container does not contain under this ID - Type: {type} \\ Id: '{id}'");
                }

                return null;
            }

            return _dictionarySingle[type][id];
        }

        public void InjectDataTo(Object obj)
        {
            if (obj == null) return;

            var listFeild = obj.GetType().GetFields(BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance).Where(x => CustomAttributeExtensions.GetCustomAttribute<InjectAttribute>((MemberInfo)x) != null);

            foreach (var field in listFeild)
            {
                var att = field.GetCustomAttribute<InjectAttribute>();
                try
                {
                    var gottenObj = Get(field.FieldType, att.Id);
                    field.SetValue(obj, gottenObj);
                }
                catch (Exception ex) { Debug.LogError(ex); }
            }

            var listProperty = obj.GetType().GetProperties(BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance).Where(x => x.GetCustomAttribute<InjectAttribute>() != null);

            foreach (var prop in listProperty)
            {
                var att = prop.GetCustomAttribute<InjectAttribute>();
                prop.SetValue(obj, Get(prop.PropertyType, att.Id));
            }

            var listMethodInfo = obj.GetType().GetMethods(BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly);

            if (listMethodInfo.Length > 0)
            {
                var methodInit = listMethodInfo.Where(x => x.GetCustomAttribute<InjectAttribute>() != null);
                if (methodInit.Count() > 0) methodInit.First().Invoke(obj, new object[0]);
            }
        }

        private void AddToDictionary(object instance, string id, Type typeInstance)
        {
            if (_dictionarySingle.ContainsKey(typeInstance))
            {
                if (_dictionarySingle[typeInstance].ContainsValue(id) == false)
                {
                    _dictionarySingle[typeInstance].Add(id, instance);
                    _logger?.Log($"Added to DI - Type: {typeInstance}- Id: '{id}'");
                }
            }
            else
            {
                _dictionarySingle.Add(typeInstance, new Dictionary<string, object>());
                _dictionarySingle[typeInstance].Add(id, instance);
            }
        }
    }
}