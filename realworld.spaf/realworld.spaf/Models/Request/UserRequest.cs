﻿using Newtonsoft.Json;

namespace realworld.spaf.Models.Request
{
    public class UserRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}