using Newtonsoft.Json;
using System;

namespace Hyperwave.Auth
{
    public class TokenInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }       
        
        [JsonIgnore]
        public DateTime LoginDate { get; set; }

        [JsonIgnore]
        public DateTime Expires
        {
            get { return LoginDate.AddSeconds(ExpiresIn); }
        }
    }
}
