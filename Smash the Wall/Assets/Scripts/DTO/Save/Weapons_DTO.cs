using DataClasses;
using Newtonsoft.Json;
using ProjectSripts;
using System.Collections.Generic;

namespace DTO.Save
{
    public sealed class Weapons_DTO : ISavable
    {
        [JsonProperty] public List<AWeapon_DTO> allWeapons { get; set; } = new List<AWeapon_DTO>();
    }
}