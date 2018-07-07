using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Response
{
    [Serializable]
    public class OwletSignInResponse
    {
        public OwletSignInResponse()
        {
        }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        private int _ExpiresIn;

        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get
            {
                return this._ExpiresIn;
            }
            set
            {
                this._ExpiresIn = value;
                this.TokenReceivedAt = DateTimeOffset.UtcNow;
            }
        }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("role_tags")]
        public IEnumerable<string> RoleTags { get; set; } = new List<string>();

        private DateTimeOffset? TokenReceivedAt { get; set; }
        private DateTimeOffset? ExpiresOn
        {
            get
            {
                if (TokenReceivedAt == null)
                {
                    return null;
                }
                return TokenReceivedAt.Value.AddSeconds(ExpiresIn);
            }
        }
        public bool IsExpired
        {
            get
            {
                return !this.ExpiresOn.HasValue || DateTimeOffset.UtcNow > ExpiresOn.Value;
            }
        }
    }
}
