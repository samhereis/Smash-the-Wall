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
    }
}