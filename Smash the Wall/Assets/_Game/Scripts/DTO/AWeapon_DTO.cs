using Newtonsoft.Json;
using System;
using UnityEngine;

namespace DTO
{
    [Serializable]
    public sealed record AWeapon_DTO
    {
        [JsonProperty][field: SerializeField] public string weaponName { get; set; }
        [JsonProperty][field: SerializeField] public bool isUnlocked { get; set; } = false;
    }
}