using ValuedInBE.Users.Models.DTOs.Request;

namespace ValuedInBE.Users.Services
{
    public interface IUserIDGenerationStrategy
    {
        Task<string> GenerateUserIDForNewUserAsync(NewUser newUser, int sameNameRepeatCount);
    }
}
