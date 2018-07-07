using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Request
{
    public class OwletSignInRequest
    {
        public OwletSignInRequest()
        {
        }

        public OwletSignInRequest(string email, string password)
            : base()
        {
            this.User.Email = email;
            this.User.Password = password;
        }

        [JsonProperty("user")]
        public OwletSignInRequestUser User { get; set; } = new OwletSignInRequestUser();
    }

    public class OwletSignInRequestUser
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("application")]
        public OwletSignInRequestApplication Application { get; set; } = new OwletSignInRequestApplication();
    }

    public class OwletSignInRequestApplication
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; } = "OWL-id";
        [JsonProperty("app_secret")]
        public string AppSecret { get; set; } = "OWL-4163742";
    }
}
