using Newtonsoft.Json;

namespace streamer.ApiModel.Auth
{
    public class TwitchAuthViewModel
    {
        public string AccessToken { get; set; }
    }
    internal class TwitchUserData
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName{ get; set; }
    }

    internal class TwitchUsersData
    {
        
        [JsonProperty("data")]
        public TwitchUserData[] Data { get; set; }
    }

    internal class TwitchUserAccessTokenData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("scope")]
        public string[] Scope { get; set; }

    }
}