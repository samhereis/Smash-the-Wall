using Sirenix.OdinInspector;
using System;

namespace Services
{
    [Serializable]
    public class Placement
    {
        [Required] public string placement;
        public AdType type;
        public DateTime lastShow;
    }
}