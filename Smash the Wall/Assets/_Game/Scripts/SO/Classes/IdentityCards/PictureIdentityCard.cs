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
        [field: SerializeField] public PictureMode pictureMode { get; private set; } = PictureMode.DestroyBorder;
        [field: SerializeField] public Color borderColor { get; private set; }

        public override async void Setup()
        {
            base.Setup();

            var target = await GetAssetAsync();

            if (target != null)
            {
                if (pictureMode != target.pictureMode) { pictureMode = target.pictureMode; }
                if (borderColor != target.borderColor) { borderColor = target.borderColor; }
            }
        }
    }
}