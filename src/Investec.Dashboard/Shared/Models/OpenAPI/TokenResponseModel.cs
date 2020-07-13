using Newtonsoft.Json;

namespace Investec.Dashboard.Shared.Models.OpenAPI
{
    public class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string Access_token { get; set; }

        [JsonProperty("token_type")]
        public string Token_type { get; set; }

        [JsonProperty("expires_in")]
        public int Expires_in { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}