using ECS.Authoring;
using System;
using UnityEngine;
using static DataClasses.Enums;

namespace IdentityCards
{
    [Serializable]
    public class PictureIdentityCard : IdentityCardBase<PictureAuthoring>
    {
        [field: SerializeField] public PictureMode pictureMode { get; private set; } = PictureMode.DestroyBorder;
        [field: SerializeField] public Color borderColor { get; private set; }

        public override void AutoSetTargetName()
        {
            base.AutoSetTargetName();
            targetName = targetName + "_" + pictureMode.ToString();
        }
    }
}