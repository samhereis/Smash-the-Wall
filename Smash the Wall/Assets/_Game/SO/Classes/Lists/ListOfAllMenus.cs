using Configs;
using System.Collections.Generic;
using UI.Canvases;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = nameof(ListOfAllMenus), menuName = "Scriptables/Lists/" + nameof(ListOfAllMenus))]
    public class ListOfAllMenus : ConfigBase
    {
        [SerializeField] private List<MenuBase> _menus = new List<MenuBase>();

        public T GetMenu<T>() where T : MenuBase
        {
            return _menus.Find(x => x.GetType() == typeof(T)) as T;
        }
    }
}