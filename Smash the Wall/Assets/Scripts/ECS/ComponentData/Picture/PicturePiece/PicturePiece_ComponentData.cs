using System;
using Unity.Entities;

namespace ECS.ComponentData.Picture.Piece
{
    [Serializable]
    public struct PicturePiece_ComponentData : IComponentData
    {
        public bool isKinematic;
        public bool isHit;
    }
}