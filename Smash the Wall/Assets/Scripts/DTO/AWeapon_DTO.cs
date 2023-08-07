using Newtonsoft.Json;

namespace DTO
{
    public sealed record AWeapon_DTO
    {
        [JsonProperty] public string weaponName { get; set; }
        [JsonProperty] public bool isUnlocked { get; set; }
    }
}