using System;
using System.Net.Http;
using System.Threading.Tasks;
using Unofficial.Owlet.Models.Response;
using Unofficial.Owlet.Models.Request;

namespace Unofficial.Owlet.EndpointClients
{
    /// <summary>
    /// See: https://developer.aylanetworks.com/apibrowser/swaggers/UserService
    /// </summary>
    public class AylaUserServiceClient : AylaServiceClient
    {
        private const string UserApiBaseaddress = "https://user-field.aylanetworks.com";

        public AylaUserServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
            httpClient.BaseAddress = new Uri(UserApiBaseaddress);
        }

        public async Task<OwletSignInResponse> LoginAsync(string email, string password)
        {
            CheckArgument(email, nameof(email));
            CheckArgument(password, nameof(password));

            var data = this.ToJsonPayload(new OwletSignInRequest(email, password));

            var response = await this.SendRequest(HttpMethod.Post, "/users/sign_in.json", data, null);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<OwletSignInResponse>(response);
        }

        public async Task<OwletSignInResponse> RefreshTokenAsync(string refreshToken)
        {
            CheckArgument(refreshToken, nameof(refreshToken));

            var data = this.ToJsonPayload(new OwletUserRefreshRequest(refreshToken));

            var response = await this.SendRequest(HttpMethod.Post, "/users/refresh_token.json", data, null);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<OwletSignInResponse>(response);
        }
    }
}
