using Microsoft.OpenApi.Extensions;
using System.Text;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Models.Users;
using ValuedInBE.Repositories;
using ValuedInBE.Security.Users;

namespace ValuedInBE.Services.Users.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserIDGenerationStrategy _userIDGeneration;

        public UserService(ILogger<UserService> logger, IUserCredentialRepository userCredentialRepository, IUserIDGenerationStrategy userIDGeneration)
        {
            _logger = logger;
            _userCredentialRepository = userCredentialRepository;
            _userIDGeneration = userIDGeneration;
        }

        public async Task<List<UserSystemInfo>> GetAllUsers()
        {
            List<UserCredentials> credentials = await _userCredentialRepository.GetAllUsers();
            return credentials.Select(MapSystemInfoFromCredentials).ToList();
        }

        public async Task<Page<UserSystemInfo>> GetUserPage(PageConfig config)
        {
            Page<UserCredentials> credentialPage =
                await _userCredentialRepository.GetUserPageWithDetails(config);

            return new Page<UserSystemInfo>(
                credentialPage.Results
                    .Select(MapSystemInfoFromCredentials).ToList(),
                credentialPage.Total,
                credentialPage.PageNo);
        }

        public async Task CreateNewUser(NewUser newUser)
        {
            if (await _userCredentialRepository.LoginExists(newUser.Login))
            {
                throw new Exception("Login already exists");
            }

            int sameNameUserCount = await _userCredentialRepository.CountWithSameName(newUser.FirstName, newUser.LastName);
            string generatedUserID = await _userIDGeneration.GenerateUserIDForNewUser(newUser, sameNameUserCount);

            UserDetails userDetails = new()
            {
                UserID = generatedUserID,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Telephone = newUser.Telephone
            };
            
            UserCredentials userCredentials = new()
            {
                UserID = generatedUserID,
                Login = newUser.Login,
                Password = newUser.Password,
                IsExpired = false,
                LastActive = null,
                Role = newUser.Role?.GetEnumFromDisplayName<UserRole>() ?? UserRole.DEFAULT,
                UserDetails = userDetails
            };

            await _userCredentialRepository.Insert(userCredentials);
        }

        public async Task<UserCredentials> GetUserCredentialsByLogin(string login)
        {
            return await _userCredentialRepository.GetByLogin(login);
        }

        public async Task<UserSystemInfo> GetUserSystemInfoByLogin(string login)
        {
            UserCredentials credentials = await _userCredentialRepository.GetByLoginWithDetails(login);
            return credentials != null ? MapSystemInfoFromCredentials(credentials) : null;
        }

        private static UserSystemInfo MapSystemInfoFromCredentials(UserCredentials credentials) =>
            new()
            {
                Login = credentials.Login,
                UserID = credentials.UserID,
                IsExpired = credentials.IsExpired,
                LastActive = credentials.LastActive,
                Role = credentials.Role.GetDisplayName(),
                FirstName = credentials.UserDetails.FirstName,
                LastName = credentials.UserDetails.LastName,
                Email = credentials.UserDetails.Email,
                Telephone = credentials.UserDetails.Telephone
            };

        public async Task ExpireUser(string login)
        {
            UserCredentials credentials = await _userCredentialRepository.GetByLogin(login);
            if (credentials == null)
                throw new KeyNotFoundException("Login does not exist");

            credentials.IsExpired = true;
            await _userCredentialRepository.Update(credentials);
        }

        public async Task UpdateUser(UpdatedUser updatedUser)
        {
            UserCredentials credentials = await _userCredentialRepository.GetByUserIdWithDetails(updatedUser.UserID); //TODO: remap
            if (credentials == null)
                throw new KeyNotFoundException("Login does not exist");

            credentials.Role = updatedUser.Role.GetEnumFromDisplayName<UserRole>();
            credentials.UserDetails.FirstName = updatedUser.FirstName;
            credentials.UserDetails.LastName = updatedUser.LastName;
            credentials.UserDetails.Email = updatedUser.Email;
            credentials.UserDetails.Telephone = updatedUser.Telephone;
            await _userCredentialRepository.Update(credentials);
        }


    }
}
