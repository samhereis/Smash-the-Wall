using DataClasses;
using Newtonsoft.Json;
using System;

namespace DTO.Save
{
    [Serializable]
    public class LevelSave_DTO : ISavable
    {
        [JsonProperty] public int pictureIndex { get; set; }
    }
}