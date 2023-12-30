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
        [ShowInInspector, ReadOnly] private readonly Dictionary<Type, Dictionary<string, object>> _dictionarySingle = new();
        [ShowInInspector, ReadOnly] private Loggers.ILogger _logger;

        public DIBox()
        {

        }

        public DIBox(Loggers.ILogger logger)
        {
            _logger = logger;
        }

        public void Clear()
        {
            _dictionarySingle.Clear();
        }

        #region Contains

        public bool Contains<T>(string id = "", bool logErrors = true)
        {
            if (_dictionarySingle.ContainsKey(typeof(T)) == false) { return false; }
            if (_dictionarySingle[typeof(T)].ContainsKey(id) == false) { return false; }

            return true;
        }

        public bool Contains(Type type, string id = "", bool logErrors = true)
        {
            if (_dictionarySingle.ContainsKey(type) == false) { return false; }
            if (_dictionarySingle[type].ContainsKey(id) == false) { return false; }

            return true;
        }

        #endregion

        #region Contains

        public void Add<T>(T instance, string id = "", bool force = false, bool asTypeProvided = false)
        {
            if (instance == null) _logger.LogWarning($"Instance is null - type {instance.GetType()} || Id: '{id}'");

            if (force == false)
            {
                if (Contains<T>(id) == true) { return; }
            }

            if (asTypeProvided == true) { AddToDictionary(instance, id, typeof(T)); }
            else { AddToDictionary(instance, id, instance.GetType()); }
        }

        #endregion

        #region Contains

        public void Remove<T>(string id = "") where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)))
            {
                _dictionarySingle[typeof(T)].Remove(id);

                _logger?.Log($"Removed from DI - Type: {typeof(T)} || Id: '{id}'");

                if (_dictionarySingle[typeof(T)].Count == 0) { _dictionarySingle.Remove(typeof(T)); }
            }
        }

        public void Remove(Type type, string id = "")
        {
            if (_dictionarySingle.ContainsKey(type))
            {
                _dictionarySingle[type].Remove(id);
                _logger?.Log($"Removed from DI - Type: {type} || Id: '{id}'");

                if (_dictionarySingle[type].Count == 0) { _dictionarySingle.Remove(type); }
            }
        }

        #endregion

        #region Contains

        public T Get<T>(string id = "", bool logErrors = true) where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)) == false)
            {
                if (logErrors == true) { _logger.LogWarning($"DI container does not contain this type  - Type: {typeof(T)} || Id: '{id}'"); }

                return null;
            }

            if (_dictionarySingle[typeof(T)].ContainsKey(id) == false)
            {
                if (logErrors == true) { _logger.LogWarning($"The container does not contain under this ID - Type: {typeof(T)} || Id: '{id}'"); }

                return null;
            }

            return _dictionarySingle[typeof(T)][id] as T;
        }

        public object Get(Type type, string id = "", bool logErrors = true)
        {
            if (_dictionarySingle.ContainsKey(type) == false)
            {
                if (logErrors == true) { _logger.LogWarning($"DI container does not contain this type  - Type: {type} || Id: '{id}'"); }

                return null;
            }

            if (_dictionarySingle[type].ContainsKey(id) == false)
            {
                if (logErrors == true) { _logger.LogWarning($"The container does not contain under this ID - Type: {type} || Id: '{id}'"); }

                return null;
            }

            return _dictionarySingle[type][id];
        }

        #endregion

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

                    _logger?.Log($"Added to DI - Type: {typeInstance} - Id: '{id}'");
                }
            }
            else
            {
                _dictionarySingle.Add(typeInstance, new Dictionary<string, object>());
                _dictionarySingle[typeInstance].Add(id, instance);

                _logger?.Log($"Added to DI - Type: {typeInstance} - Id: '{id}'");
            }
        }
    }
}