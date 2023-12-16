using Configs;
using DataClasses;
using Helpers;
using Interfaces;
using UnityEngine;

namespace SO.Lists
{
    [CreateAssetMenu(fileName = "ListOfAllScenes", menuName = "Scriptables/Lists/ListOfAllScenes")]
    public class ListOfAllScenes : ConfigBase, IInitializable
    {
        [field: SerializeField] public AScene mainMenuScene { get; private set; }
        [field: SerializeField] public AScene gameScene { get; private set; }

        public override void Initialize()
        {
            mainMenuScene.Initialize();
            gameScene.Initialize();

            this.TrySetDirty();
        }
    }
}