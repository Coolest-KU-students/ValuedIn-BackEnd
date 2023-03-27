using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Services
{
    public interface IUserService
    {
        public Task<Page<UserSystemInfo>> GetUserPageAsync(UserPageRequest config);
        public Task<UserCredentials> GetUserCredentialsByLoginAsync(string login);
        public Task<UserSystemInfo> GetUserSystemInfoByLoginAsync(string login);
        Task ExpireUserAsync(string login);
        Task UpdateUserAsync(UpdatedUser updatedUser);
        Task UpdateLastActiveAsync(string login);
        Task UpdateLastActiveAsync(UserCredentials userCredentials);
        Task UpdateLastActiveByUserIDAsync(string userID);
        Task CreateNewUserAsync(NewUser newUser, UserContext userContext = null);
        Task<bool> VerifyUserIdExistenceAsync(List<string> userIds);
    }
}
