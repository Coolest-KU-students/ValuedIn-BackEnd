using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Services.Users
{
    public interface IUserService
    {
        public Task<Page<UserSystemInfo>> GetUserPage(UserPageRequest config);
        public Task<UserCredentials> GetUserCredentialsByLogin(string login);
        public Task<UserSystemInfo> GetUserSystemInfoByLogin(string login);
        Task ExpireUser(string login);
        Task UpdateUser(UpdatedUser updatedUser);
        Task UpdateLastActiveByLogin(string login);
        Task UpdateLastActiveByUserID(string userID);
        Task CreateNewUser(NewUser newUser, UserContext userContext = null);
    }
}
