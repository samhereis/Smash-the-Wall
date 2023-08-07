using DataClasses;
using Newtonsoft.Json;
using System;

namespace DTO.Save
{
    [Serializable]
    public class LevelSave_DTO : ISavable
    {
        [JsonProperty] public int levelIndex { get; set; }
        [JsonProperty] public int pictureIndex { get; set; }
        [JsonProperty] public int sceneIndex { get; set; }
    }
}