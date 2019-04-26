using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeInspection.Models.Common
{
    public class LoginUser
    {
        public LoginUser()
        {

        }
        [JsonProperty("compnm")]
        public string Compnm { get; set; }

        [JsonProperty("wareky")]
        public string Wareky { get; set; }

        [JsonProperty("warenm")]
        public string Warenm { get; set; }

        [JsonProperty("userid")]
        public string Userid { get; set; }

        [JsonProperty("passwd")]
        public string Passwd { get; set; }

        [JsonProperty("auth_token")]
        public string Auth_Token { get; set; }

        [JsonProperty("compky")]
        public string Compky { get; set; }
    }
}
