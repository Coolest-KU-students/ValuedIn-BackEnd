using ValuedInBE.Models.DTOs.Requests.Users;

namespace ValuedInBE.Services.Users
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateUser(AuthRequest auth);
        Task RegisterNewUser(NewUser newUser);
        Task SelfRegister(NewUser newUser);
    }
}
