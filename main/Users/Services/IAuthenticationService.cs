using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Services
{
    public interface IAuthenticationService
    {
        Task<TokenAndRole> AuthenticateUserAsync(AuthRequest auth);
        Task RegisterNewUserAsync(NewUser newUser);
        Task SelfRegisterAsync(NewUser newUser);
        Task<TokenAndRole> VerifyAndIssueNewTokenAsync(string token);
        Task<UserCredentials> GetUserFromTokenAsync(string token);
    }
}
