using Configs;
using Helpers;
using IdentityCards;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Lists
{
    [Serializable]
    [CreateAssetMenu(fileName = "ListOfAllScenes", menuName = "SO/Lists/ListOfAllScenes")]
    public class ListOfAllScenes : ConfigBase
    {
        [field: SerializeField] public List<SceneIdentityCard> scenes { get; private set; } = new List<SceneIdentityCard>();
        [field: SerializeField] public int currentSceneIndex { get; private set; } = 0;

        public override void Initialize()
        {

        }

        public SceneIdentityCard GetRandom()
        {
            return scenes.GetRandom();
        }

        public SceneIdentityCard GetNext()
        {
            currentSceneIndex++;

            if (currentSceneIndex >= scenes.Count)
            {
                currentSceneIndex = 0;
            }

            return scenes[currentSceneIndex];
        }
    }
}