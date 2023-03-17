using ValuedInBE.Models;

namespace ValuedInBE.Services.Tokens
{
    public interface ITokenService
    {
        string GenerateOneTimeUserAccessToken(UserContext userContext, string type, TimeSpan? timeSpan = null);
        UserContext GetUserContextFromToken(string token, string type);
    }
}
