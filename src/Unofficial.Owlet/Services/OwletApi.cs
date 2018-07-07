using System;
using System.Threading.Tasks;
using Unofficial.Owlet.EndpointClients;
using Unofficial.Owlet.Interfaces;
using Unofficial.Owlet.Models;

namespace Unofficial.Owlet.Services
{
    public class OwletApi : IOwletApi
    {
        protected readonly IOwletApiSettings _owletApiSettings;
        protected readonly OwletUserSession _owletUserSession;

        private readonly AylaUserServiceClient _aylaUserServiceClient;

        public OwletApi(IOwletApiSettings owletApiSettings, OwletUserSession owletUserSession, AylaUserServiceClient aylaUserServiceClient)
        {
            this._owletApiSettings = owletApiSettings;
            this._owletUserSession = owletUserSession;
            this._aylaUserServiceClient = aylaUserServiceClient;
        }

        public async Task<string> GetAccessToken(string accessToken = null, string email = null, string password = null)
        {
            // short-circuit if provided
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                return accessToken;
            }

            // short-circuit if available in current user session (and not expired)
            if (!string.IsNullOrWhiteSpace(this._owletUserSession?.SignInResponse?.AccessToken))
            {
                if (this._owletUserSession?.SignInResponse?.IsExpired == false)
                {
                    return this._owletUserSession?.SignInResponse?.AccessToken;
                }
                else if (!string.IsNullOrWhiteSpace(this._owletUserSession?.SignInResponse?.RefreshToken))
                {
                    // TODO: can we refresh instead of re-logging in?
                }
            }

            // try logging in
            if (string.IsNullOrWhiteSpace(email))
            {
                email = this._owletApiSettings.Email;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                password = this._owletApiSettings.Password;
            }

            var signInResponse = await this._aylaUserServiceClient.LoginAsync(email, password);

            if (signInResponse == null)
            {
                throw new ArgumentException("Must run IOwletUserApi.LoginAsync(username, password) before executing authorized endpoints without providing an access_token explicitly.");
            }

            this._owletUserSession?.SetSession(signInResponse);

            return this._owletUserSession?.SignInResponse.AccessToken;
        }
    }
}
