using Configs;
using ErtenGamesInstrumentals.DataClasses;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Canvases;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = nameof(ListOfAllMenus), menuName = "Scriptables/Lists/" + nameof(ListOfAllMenus))]
    public class ListOfAllMenus : ConfigBase
    {
        [ListDrawerSettings(ListElementLabelName = nameof(PrefabReference<MenuBase>.targetTypeName))]
        [SerializeField] private List<PrefabReference<MenuBase>> _menus = new List<PrefabReference<MenuBase>>();

        public T GetMenu<T>() where T : MenuBase
        {
            var reference = _menus.Find(x => x.targetTypeName == typeof(T).Name);

            return reference.GetAssetComponent<T>();
        }

        public async Task<T> GetMenuAsync<T>() where T : MenuBase
        {
            var reference = _menus.Find(x => x.targetTypeName == typeof(T).Name);

            return await reference.GetAssetComponentAsync<T>();
        }
    }
}