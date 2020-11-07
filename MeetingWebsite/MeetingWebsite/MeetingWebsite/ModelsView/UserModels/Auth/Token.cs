using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.ModelsView.Auth
{
    public class Token
    {
        public Token(string access_token, string username)
        {
            AccessToken = access_token;
            Username = username;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
