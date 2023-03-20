using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Services
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
