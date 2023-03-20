using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Services
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
        Task<bool> VerifyUserIdExistenceAsync(List<string> userIds);
    }
}
