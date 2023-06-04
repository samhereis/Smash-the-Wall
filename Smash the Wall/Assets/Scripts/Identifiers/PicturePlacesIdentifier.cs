using System.Collections.Generic;
using UnityEngine;

namespace Identifiers
{
    public class PicturePlacesIdentifier : IdentifierBase
    {
        [field: SerializeField] public List<Transform> picturePlaces { get; private set; }
    }
}