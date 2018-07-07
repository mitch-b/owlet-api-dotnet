using System.Threading.Tasks;
using Unofficial.Owlet.Models.Response;

namespace Unofficial.Owlet.Interfaces
{
    /// <summary>
    /// Provides access to user-level information of your Owlet account, including authentication tasks.
    /// </summary>
    public interface IOwletUserApi
    {
        /// <summary>
        /// Perform login event. Login information is persisted for the lifecycle of the <see cref="Unofficial.Owlet.Models.OwletUserSession"/>.
        /// </summary>
        /// <param name="email">Your Owlet account email</param>
        /// <param name="password">Your Owlet account password</param>
        /// <returns></returns>
        Task<OwletSignInResponse> LoginAsync(string email, string password);
        /// <summary>
        /// Perform token refresh. Login information is persisted for the lifecycle of the <see cref="Unofficial.Owlet.Models.OwletUserSession"/>.
        /// </summary>
        /// <param name="refreshToken">From initial login event</param>
        /// <returns></returns>
        Task<OwletSignInResponse> RefreshLoginAsync(string refreshToken = null);
    }
}
