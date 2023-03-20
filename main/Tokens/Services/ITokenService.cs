using ValuedInBE.Users.Models;

namespace ValuedInBE.Tokens.Services
{
    public interface ITokenService
    {
        string GenerateOneTimeUserAccessToken(UserContext userContext, string type, TimeSpan? timeSpan = null);
        UserContext GetUserContextFromToken(string token, string type);
    }
}
