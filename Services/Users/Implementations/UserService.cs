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
        private ILogger<UserService> _logger;
        private IUserCredentialRepository _userCredentialRepository;

        public UserService(ILogger<UserService> logger, IUserCredentialRepository userCredentialRepository)
        {
            _logger = logger;
            _userCredentialRepository = userCredentialRepository;
        }

        public async Task<List<UserSystemInfo>> GetAllUsers()
        {
            List<UserCredentials> credentials = await _userCredentialRepository.GetAllUsers();
            return credentials.Select(MapSystemInfoFromCredentials).ToList();
        }

        public async Task<Page<UserSystemInfo>> GetUserPage(PageConfig config)
        {
            Page<UserCredentials> credentialPage = await _userCredentialRepository.GetUserPageWithDetails(config);
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

            UserDetails userDetails = new(newUser.Login, newUser.FirstName, newUser.LastName, newUser.Email, newUser.Telephone);
            UserCredentials userCredentials = new(newUser.Login, newUser.Password, false, null, newUser.Role ?? UserRole.DEFAULT, userDetails);

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
                IsExpired = credentials.IsExpired,
                LastActive = credentials.LastActive,
                Role = credentials.Role,
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
            UserCredentials credentials = await _userCredentialRepository.GetByLoginWithDetails(updatedUser.Login);
            if (credentials == null)
                throw new KeyNotFoundException("Login does not exist");

            credentials.Role = updatedUser.Role;
            credentials.UserDetails.FirstName = updatedUser.FirstName;
            credentials.UserDetails.LastName = updatedUser.LastName;
            credentials.UserDetails.Email = updatedUser.Email;
            credentials.UserDetails.Telephone = updatedUser.Telephone;
            await _userCredentialRepository.Update(credentials);
        }



    }
}
