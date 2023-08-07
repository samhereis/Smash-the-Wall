using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace DI
{
    public static class DIBox
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> _dictionarySingle = new Dictionary<Type, Dictionary<string, object>>();

        public static void Clear()
        {
            _dictionarySingle.Clear();
        }

        public static void Add<T>(T instance, string id = "")
        {
            if (instance == null) Debug.LogWarning($"Instance is null - type {instance.GetType()}");

            AddToDictionary(instance, id, instance.GetType());
        }

        public static void Add(Object instance, string id = "")
        {
            if (instance == null) Debug.LogWarning($"Instance is null");

            Type typeInstance = instance.GetType();

            AddToDictionary(instance, id, typeInstance);
        }

        public static void Remove<T>(string id = "") where T : class
        {
            if (_dictionarySingle.ContainsKey(typeof(T)))
            {
                _dictionarySingle[typeof(T)].Remove(id);

                //Debug.Log($"Removed from DI - Type: {typeof(T)} - Id: '{id}'");
            }
            if (_dictionarySingle.Count == 0)
            {
                _dictionarySingle.Remove(typeof(T));
            }
        }

        public static void Remove(Type type, string id = "")
        {
            if (_dictionarySingle.ContainsKey(type))
            {
                _dictionarySingle[type].Remove(id);
                //Debug.Log($"Removed from DI - Type: {type} - Id: '{id}'");
            }
            if (_dictionarySingle.Count == 0)
            {
                _dictionarySingle.Remove(type);
            }
        }

        public static T Get<T>(string id = "", bool logErrors = true) where T : class
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

        public static object Get(Type type, string id = "", bool logErrors = true)
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

        public static void InjectDataTo(Object obj)
        {
            if (obj == null) return;

            var listFeild = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => CustomAttributeExtensions.GetCustomAttribute<DIAttribute>((MemberInfo)x) != null);

            foreach (var field in listFeild)
            {
                var att = field.GetCustomAttribute<DIAttribute>();
                try
                {
                    var gottenObj = Get(field.FieldType, att.Id);
                    field.SetValue(obj, gottenObj);
                }
                catch (Exception ex) { Debug.LogError(ex); }
            }

            var listProperty = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.GetCustomAttribute<DIAttribute>() != null);

            foreach (var prop in listProperty)
            {
                var att = prop.GetCustomAttribute<DIAttribute>();
                prop.SetValue(obj, Get(prop.PropertyType, att.Id));
            }

            var listMethodInfo = obj.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (listMethodInfo.Length > 0)
            {
                var methodInit = listMethodInfo.Where(x => x.GetCustomAttribute<DIAttribute>() != null);
                if (methodInit.Count() > 0) methodInit.First().Invoke(obj, new object[0]);
            }
        }

        private static void AddToDictionary(object instance, string id, Type typeInstance)
        {
            if (_dictionarySingle.ContainsKey(typeInstance))
            {
                if (_dictionarySingle[typeInstance].ContainsValue(id) == false)
                {
                    _dictionarySingle[typeInstance].Add(id, instance);
                    //Debug.Log($"Added to DI - Type: {typeInstance}- Id: '{id}'");
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