using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers
{
    public static class CollectionsHelper
    {

        #region List

        public static void RemoveNulls<T>(this List<T> list)
        {
            list.RemoveAll(x => x == null);
        }

        public static void SafeAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false) list.Add(item);
        }

        public static void SafeRemove<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == true) list.Remove(item);
        }

        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var listCopy = new HashSet<T>();

            foreach (var item in list.ToHashSet())
            {
                listCopy.Add(item);
            }

            list = listCopy.ToList();
        }

        public async static
#if UNITY_2023_2_OR_NEWER
            Awaitable
#else
            Task
#endif
            RemoveDuplicatesAsync<T>(this List<T> list)
        {
            var listCopy = new List<T>();
            listCopy.AddRange(list);

            foreach (T itemToCheck in listCopy)
            {
                foreach (T itemToPotentiallyRemove in listCopy)
                {
                    bool isEqual = itemToPotentiallyRemove.Equals(itemToCheck);

                    if (isEqual) list.Remove(itemToPotentiallyRemove);

                    await AsyncHelper.Skip();
                }
            }
        }

        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0) return default(T);

            return list.ElementAt(Random.Range(0, list.Count()));
        }

        public static int GetRandomIndex<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0) return 0;
            return Random.Range(0, list.Count());
        }

        public static bool HasEnoughElementsForIndex<T>(this IEnumerable<T> list, int index)
        {
            return list.Count() >= index;
        }

        #endregion

        #region Array

        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        #endregion
    }
}