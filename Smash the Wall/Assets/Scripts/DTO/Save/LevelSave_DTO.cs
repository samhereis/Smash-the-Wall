using DataClasses;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace DTO.Save
{
    [Serializable]
    public class LevelSave_DTO : ISavable
    {
        [JsonProperty] public int pictureIndex { get; set; }
    }
}