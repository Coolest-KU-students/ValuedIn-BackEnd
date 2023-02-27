using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Repositories
{
    public interface IUserCredentialRepository
    {
        Task<Page<UserCredentials>> GetUserPageWithDetails(PageConfig config);
        Task<UserCredentials> GetByLogin(String login);
        Task<List<UserCredentials>> GetAllUsers();
        Task Update(UserCredentials userCredentials);
        Task Insert(UserCredentials userCredentials);
        Task<bool> LoginExists(string login);
        Task<UserCredentials> GetByLoginWithDetails(string login);
        Task<int> CountWithSameName(string firstName, string lastName);
        Task<UserCredentials> GetByUserIdWithDetails(string userID);
    }
}
