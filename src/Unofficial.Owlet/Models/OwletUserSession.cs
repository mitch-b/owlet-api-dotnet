using Unofficial.Owlet.Models.Response;

namespace Unofficial.Owlet.Models
{
    public class OwletUserSession
    {
        public OwletSignInResponse SignInResponse { get; private set; }

        public OwletUserSession()
        {
        }

        public void SetSession(OwletSignInResponse value)
        {
            this.SignInResponse = value;
        }

        public void ClearSession()
        {
            this.SignInResponse = default(OwletSignInResponse);
        }
    }
}
