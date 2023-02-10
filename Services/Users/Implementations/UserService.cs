using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests;
using ValuedInBE.Models.DTOs.Responses;
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
            return credentials.Select(mapCredentialsToUser).ToList();
        }

        public async Task<Page<UserSystemInfo>> GetUserPage(PageConfig config)
        {
            Page<UserCredentials> credentialPage = await _userCredentialRepository.GetUserPage(config);
            return new Page<UserSystemInfo>(credentialPage.Results.Select(mapCredentialsToUser).ToList(), credentialPage.Total, credentialPage.PageNo);
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

        private Func<UserCredentials, UserSystemInfo> mapCredentialsToUser => (credentials) =>
            new UserSystemInfo(credentials.Login,
                        credentials.IsExpired,
                        credentials.LastActive,
                        credentials.Role ,
                        credentials.UserDetails.FirstName,
                        credentials.UserDetails.LastName,
                        credentials.UserDetails.Email,
                        credentials.UserDetails.Telephone
                    );
    
    }
}
