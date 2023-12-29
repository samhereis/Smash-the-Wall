using ECS.Authoring;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using static DataClasses.Enums;

namespace IdentityCards
{
    [Serializable]
    public class PictureIdentityCard : IdentityCardBase<PictureAuthoring>
    {
        [ShowInInspector] public PictureMode pictureMode { get; private set; } = PictureMode.DestroyBorder;
        [ShowInInspector] public Color borderColor { get; private set; }

        public override void Validate()
        {
            base.Validate();

            if (target != null)
            {
                targetName = targetName + "_" + pictureMode.ToString();

                if (pictureMode != target.pictureMode) { pictureMode = target.pictureMode; }
                if (borderColor != target.borderColor) { borderColor = target.borderColor; }
            }
        }
    }
}