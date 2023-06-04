using Newtonsoft.Json;
using System;

namespace Authorization.Data.Models
{
    [Serializable]
    internal class SignInUserModel
    {
        [JsonProperty] public string email { get; set; }
        [JsonProperty] public string password { get; set; }

        public SignInUserModel(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}