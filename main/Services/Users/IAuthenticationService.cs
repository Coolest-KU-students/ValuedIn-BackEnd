using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Services.Users
{
    public interface IAuthenticationService
    {
        Task<TokenAndRole> AuthenticateUser(AuthRequest auth);
        Task RegisterNewUser(NewUser newUser);
        Task SelfRegister(NewUser newUser);
        Task<TokenAndRole> VerifyToken(string token);
        Task<UserCredentials> GetUserFromToken(string token);
    }
}
