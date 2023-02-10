using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests;
using ValuedInBE.Models.DTOs.Responses;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Services.Users
{
    public interface IUserService
    {
        public Task<Page<UserSystemInfo>> GetUserPage(PageConfig config);

        public Task<List<UserSystemInfo>> GetAllUsers();

        public Task CreateNewUser(NewUser newUser);

        public Task<UserCredentials> GetUserCredentialsByLogin(string Login);
    }
}
