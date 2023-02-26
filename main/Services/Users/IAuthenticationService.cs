using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;

namespace ValuedInBE.Services.Users
{
    public interface IAuthenticationService
    {
        Task<TokenAndRole> AuthenticateUser(AuthRequest auth);
        Task RegisterNewUser(NewUser newUser);
        Task SelfRegister(NewUser newUser);
        Task<TokenAndRole> VerifyToken(string token);
    }
}
