using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Repositories
{
    public interface IUserCredentialRepository
    {
        Task<Page<UserCredentials>> GetUserPageWithDetails(UserPageRequest config);
        Task<UserCredentials> GetByLogin(string login);
        Task<UserCredentials> GetByUserID(string userID);
        Task<List<UserCredentials>> GetAllUsers();
        Task Update(UserCredentials userCredentials, UserContext updatedBy);
        Task Insert(UserCredentials userCredentials, UserContext createdBy);
        Task<bool> LoginExists(string login);
        Task<UserCredentials> GetByLoginWithDetails(string login);
        Task<int> CountWithNames(string firstName, string lastName);
        Task<UserCredentials> GetByUserIdWithDetails(string userID);
        Task<bool> VerifyUserIds(List<string> userIds);
    }
}
