
#if NewtonsoftInstalled
using Newtonsoft.Json;
#endif

using System;

namespace Authorization.Models
{
    [Serializable]
    internal class SignInUserModel
    {

#if NewtonsoftInstalled
        [JsonProperty]
#endif
        public string email { get; set; }

#if NewtonsoftInstalled
        [JsonProperty]
#endif
        public string password { get; set; }

        public SignInUserModel(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
