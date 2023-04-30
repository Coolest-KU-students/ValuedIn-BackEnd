using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Repositories
{
    public interface IUserValueRepository
    {
        Task<IEnumerable<UserValue>> GetAllUserPersonalValuesForUserAsync(string userId);
        Task CreateUserValueAsync(UserValue userValue);
        Task RemoveUserValueAsync(UserValue userValue);

    }
}
