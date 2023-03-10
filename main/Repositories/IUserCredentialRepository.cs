using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Repositories
{
    public interface IUserCredentialRepository
    {
        Task<Page<UserCredentials>> GetUserPageWithDetails(UserPageRequest config);
        Task<UserCredentials> GetByLogin(string login);
        Task<UserCredentials> GetByUserID(string userID);
        Task<List<UserCredentials>> GetAllUsers();
        Task Update(UserCredentials userCredentials);
        Task Insert(UserCredentials userCredentials);
        Task<bool> LoginExists(string login);
        Task<UserCredentials> GetByLoginWithDetails(string login);
        Task<int> CountWithNames(string firstName, string lastName);
        Task<UserCredentials> GetByUserIdWithDetails(string userID);
    }
}
