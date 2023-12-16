using DataClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DTO.Save
{
    [Serializable]
    public sealed class Weapons_DTO : ISavable
    {
        [JsonProperty][field: SerializeField] public List<AWeapon_DTO> allWeapons { get; set; } = new List<AWeapon_DTO>();
        [JsonProperty][field: SerializeField] public int currentWeaponIndex { get; set; }
    }
}