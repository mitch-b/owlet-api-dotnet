using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Request
{
    public class OwletUserRefreshRequest
    {
        public OwletUserRefreshRequest()
        {
        }
        public OwletUserRefreshRequest(string refreshToken)
        {
            User = new OwletUserRefreshToken(refreshToken);
        }
        [JsonProperty("user")]
        public OwletUserRefreshToken User { get; set; } = new OwletUserRefreshToken();
    }

    public class OwletUserRefreshToken
    {
        public OwletUserRefreshToken()
        {
        }
        public OwletUserRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
