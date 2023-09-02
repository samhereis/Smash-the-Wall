using DataClasses;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace DTO.Save
{
    [Serializable]
    public class LevelSave_DTO : ISavable
    {
        [JsonProperty][field: SerializeField] public int levelIndex { get; set; } = 0;
        [JsonProperty][field: SerializeField] public int pictureIndex { get; set; } = 0;
    }
}