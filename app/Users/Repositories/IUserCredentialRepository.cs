using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Repositories
{
    public interface IUserCredentialRepository
    {
        Task<Page<UserCredentials>> GetUserPageWithDetailsAsync(UserPageRequest config);
        Task<UserCredentials?> GetByLoginAsync(string login);
        Task<UserCredentials?> GetByUserIDAsync(string userID);
        Task<IEnumerable<UserCredentials>> GetAllUsersAsync();
        Task UpdateAsync(UserCredentials userCredentials, UserContext updatedBy);
        Task InsertAsync(UserCredentials userCredentials, UserContext createdBy);
        Task<bool> LoginExistsAsync(string login);
        Task<UserCredentials?> GetByLoginWithDetailsAsync(string login);
        Task<int> CountWithSameNamesAsync(string firstName, string lastName);
        Task<UserCredentials?> GetByUserIdWithDetailsAsync(string userID);
        Task<bool> VerifyUserIdsAsync(IEnumerable<string> userIds);
    }
}
