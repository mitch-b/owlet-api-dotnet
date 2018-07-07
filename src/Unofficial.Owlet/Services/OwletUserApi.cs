using System.Threading.Tasks;
using Unofficial.Owlet.EndpointClients;
using Unofficial.Owlet.Interfaces;
using Unofficial.Owlet.Models;
using Unofficial.Owlet.Models.Response;
using Unofficial.Owlet.Utils;

namespace Unofficial.Owlet.Services
{
    public class OwletUserApi : OwletApi, IOwletUserApi
    {
        private readonly AylaUserServiceClient _aylaUserServiceClient;

        public OwletUserApi(AylaUserServiceClient aylaUserServiceClient, IOwletApiSettings owletApiSettings, OwletUserSession owletUserSession)
            : base(owletApiSettings, owletUserSession, aylaUserServiceClient)
        {
            this._aylaUserServiceClient = aylaUserServiceClient;
        }

        public async Task<OwletSignInResponse> LoginAsync(string email, string password)
        {
            var signInResponse = await this._aylaUserServiceClient.LoginAsync(email, password);
            this._owletUserSession?.SetSession(ObjectCopier.Clone(signInResponse));
            return signInResponse;
        }

        public async Task<OwletSignInResponse> RefreshLoginAsync(string refreshToken = null)
        {
            // short-circuit if provided
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                refreshToken = this._owletUserSession?.SignInResponse?.RefreshToken;
            }
            var signInResponse = await this._aylaUserServiceClient.RefreshTokenAsync(refreshToken);
            this._owletUserSession?.SetSession(ObjectCopier.Clone(signInResponse));
            return signInResponse;
        }
    }
}
