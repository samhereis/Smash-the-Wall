using Newtonsoft.Json;
using System;

namespace DataClasses
{
    [Serializable]
    public class Token
    {
        private const string FOLDER_NAME = "DataBase";
        private const string FILE_NAME = "Token";

        [JsonProperty] public string token { get; set; }

        public Token(string token)
        {
            this.token = token;
        }
    }
}