using AutoMapper;
using Microsoft.OpenApi.Extensions;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.System.Security.Users;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Exceptions;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Repositories;

namespace ValuedInBE.Users.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserIDGenerationStrategy _userIDGeneration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public UserService(ILogger<UserService> logger, IUserCredentialRepository userCredentialRepository, IUserIDGenerationStrategy userIDGeneration, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _logger = logger;
            _userCredentialRepository = userCredentialRepository;
            _userIDGeneration = userIDGeneration;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<Page<UserSystemInfo>> GetUserPageAsync(UserPageRequest config)
        {
            _logger.LogDebug("Fetching User Page {pageNo} with size {size}", config.Page, config.Size);
            Page<UserCredentials> credentialPage =
                await _userCredentialRepository.GetUserPageWithDetailsAsync(config);

            return new Page<UserSystemInfo> {
                Results = credentialPage.Results
                    .Select(MapSystemInfoFromCredentials),
                Total = credentialPage.Total,
                PageNo = credentialPage.PageNo};
        }

        public async Task CreateNewUserAsync(NewUser newUser, UserContext? userContext = null)
        {
            _logger.LogDebug("Creating new user with login {login}", newUser.Login);

            if (await _userCredentialRepository.LoginExistsAsync(newUser.Login))
            {
                _logger.LogTrace("Tried to create a new user, but {login} was already taken", newUser.Login);
                throw new CredentialsExistException("login", newUser.Login);
            }

            int sameNameUserCount = await _userCredentialRepository.CountWithSameNamesAsync(newUser.FirstName, newUser.LastName);
            string generatedUserID = await _userIDGeneration.GenerateUserIDForNewUserAsync(newUser, sameNameUserCount);
            UserRoleExtended role = UserRoleExtended.FromString(newUser.Role) ?? UserRoleExtended.DEFAULT;
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
                Role = role,
                UserDetails = userDetails
            };

            userContext ??= _contextAccessor.HttpContext?.GetUserContext();
            if (userContext == null)
            {
                //TODO: I question this security myself, should throw an error, but then initial Seed will break... Maybe I should try attaching a context there
                _logger.LogWarning("User is being created with Login {login}, ID {userId} and Role {role}, but there is no User Context present. ", newUser.Login, generatedUserID, role);
                userContext = new()
                {
                    Login = "Unknown",
                    UserID = "Unknown",
                    Role = UserRole.SYS_ADMIN //since that is basically what it is given
                };
            }
            await _userCredentialRepository.InsertAsync(userCredentials, userContext);
        }

        public async Task<UserCredentials?> GetUserCredentialsByLoginAsync(string login)
        {
            _logger.LogDebug("Fetching user credentials for login {login} ", login);
            return await _userCredentialRepository.GetByLoginAsync(login);
        }

        public async Task<UserSystemInfo?> GetUserSystemInfoByLoginAsync(string login)
        {
            _logger.LogDebug("Fetching user system info for login {login} ", login);
            UserCredentials? credentials = await _userCredentialRepository.GetByLoginWithDetailsAsync(login);
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
                FirstName = credentials.UserDetails?.FirstName ?? throw new Exception("Did not retrieve user details"),
                LastName = credentials.UserDetails.LastName,
                Email = credentials.UserDetails.Email,
                Telephone = credentials.UserDetails.Telephone
            };

        public async Task ExpireUserAsync(string login)
        {
            _logger.LogDebug("Tying to expired user with {login} ", login);
            UserContext userContext = GetUserContextFromHttpOrThrowException();
            UserCredentials? credentials = await _userCredentialRepository.GetByLoginAsync(login);
            if (credentials == null)
            {
                _logger.LogTrace("Did not find a user with login {login}", login);
                throw new KeyNotFoundException("Login does not exist");
            }

            credentials.IsExpired = true;

            await _userCredentialRepository.UpdateAsync(credentials, userContext);
        }

        public async Task UpdateUserAsync(UpdatedUser updatedUser)
        {
            _logger.LogDebug("Tying to update user with userId {userId} ", updatedUser.UserID);
            UserContext userContext = GetUserContextFromHttpOrThrowException();
            UserCredentials? credentials = await _userCredentialRepository.GetByUserIdWithDetailsAsync(updatedUser.UserID);
            if (credentials == null)
            {
                _logger.LogTrace("Did not find a user with userId {login}", updatedUser.UserID);
                throw new KeyNotFoundException("Login does not exist");
            }

            credentials.Role = UserRoleExtended.FromString(updatedUser.Role) ?? throw new Exception("Role was not recognized");
            credentials.UserDetails!.FirstName = updatedUser.FirstName;
            credentials.UserDetails.LastName = updatedUser.LastName;
            credentials.UserDetails.Email = updatedUser.Email;
            credentials.UserDetails.Telephone = updatedUser.Telephone;

            await _userCredentialRepository.UpdateAsync(credentials, userContext);
        }

        public async Task UpdateLastActiveAsync(string login)
        {
            UserCredentials credentials =
                await _userCredentialRepository.GetByLoginAsync(login)
                ?? throw new Exception("Login was not find");
            await UpdateLastActiveAsync(credentials);
        }

        public async Task UpdateLastActiveAsync(UserCredentials userCredentials)
        {
            UserContext userContext = _mapper.Map<UserContext>(userCredentials);
            userCredentials.LastActive = DateTime.Now;
            await _userCredentialRepository.UpdateAsync(userCredentials, userContext);
        }

        public async Task UpdateLastActiveByUserIDAsync(string userID)
        {
            UserCredentials credentials = 
                await _userCredentialRepository.GetByUserIDAsync(userID)
                ?? throw new Exception("UserID was not found");
            UserContext userContext = _mapper.Map<UserContext>(credentials);
            credentials.LastActive = DateTime.Now;
            await _userCredentialRepository.UpdateAsync(credentials, userContext);
        }

        private UserContext GetUserContextFromHttpOrThrowException()
        {
            return _contextAccessor.HttpContext?.GetUserContext()
                   ?? throw new Exception("User could not be recognized by the system; Cannot execute");
        }

        public async Task<bool> VerifyUserIdExistenceAsync(List<string> userIds)
        {
            return await _userCredentialRepository.VerifyUserIdsAsync(userIds);
        }
    }
}
