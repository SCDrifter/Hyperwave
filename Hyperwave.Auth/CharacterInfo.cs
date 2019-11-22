﻿using System;
using Newtonsoft.Json;

namespace Hyperwave.Auth
{
    public class CharacterInfo
    {
        [JsonProperty("CharacterID")]
        public long CharacterId { get; set; }

        [JsonProperty("CharacterName")]
        public string CharacterName { get; set; }

        [JsonProperty("ExpiresOn")]
        public DateTime Expires { get; set; }

        [JsonProperty("Scopes")]
        public string Scopes { get; set; }

        [JsonProperty("TokenType")]
        public string TokenType { get; set; }

        [JsonProperty("CharacterOwnerHash")]
        public string CharacterOwnerHash { get; set; }
    }
}
