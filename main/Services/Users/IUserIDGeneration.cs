using ValuedInBE.Models.DTOs.Requests.Users;

namespace ValuedInBE.Services.Users
{
    public interface IUserIDGenerationStrategy
    {
        Task<string> GenerateUserIDForNewUser(NewUser newUser, int sameNameRepeatCount);


    }
}
