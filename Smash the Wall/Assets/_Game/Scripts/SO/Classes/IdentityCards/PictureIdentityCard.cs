using ECS.Authoring;
using ErtenGamesInstrumentals.DataClasses;
using System;
using UnityEngine;
using static DataClasses.Enums;

namespace IdentityCards
{
    [Serializable]
    public class PictureIdentityCard : PrefabReference<PictureAuthoring>
    {
        [SerializeField] public PictureMode pictureMode { get; private set; } = PictureMode.DestroyBorder;
        [SerializeField] public Color borderColor { get; private set; }
        [SerializeField] public string targetName { get; private set; }


#if UNITY_EDITOR
        public PictureIdentityCard(PictureAuthoring resourceReference) : base(resourceReference)
        {

        }
#endif

        public override async void Setup()
        {
            base.Setup();

            var target = await GetAssetAsync();

            if (target != null)
            {
                targetName = pictureMode.ToString() + "_" + target.name;

                if (pictureMode != target.pictureMode) { pictureMode = target.pictureMode; }
                if (borderColor != target.borderColor) { borderColor = target.borderColor; }
            }
        }
    }
}