using AutoMapper;
using Microsoft.OpenApi.Extensions;
using NuGet.Protocol.Plugins;
using ValuedInBE.DataControls.Memory;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Models.Users;
using ValuedInBE.Repositories;
using ValuedInBE.Security.Users;
using ValuedInBE.System;

namespace ValuedInBE.Services.Users.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserIDGenerationStrategy _userIDGeneration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoizationEngine _memoizationEngine;
        private readonly IMapper _mapper;


        public UserService(ILogger<UserService> logger, IUserCredentialRepository userCredentialRepository, IUserIDGenerationStrategy userIDGeneration, IMemoizationEngine memoizationEngine, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _logger = logger;
            _userCredentialRepository = userCredentialRepository;
            _userIDGeneration = userIDGeneration;
            _memoizationEngine = memoizationEngine;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<Page<UserSystemInfo>> GetUserPage(UserPageRequest config)
        {
            _logger.LogDebug("Fetching User Page {pageNo} with size {size}", config.Page, config.Size);
            Page<UserCredentials> credentialPage =
                await _userCredentialRepository.GetUserPageWithDetails(config);

            return new Page<UserSystemInfo>(
                credentialPage.Results
                    .Select(MapSystemInfoFromCredentials).ToList(),
                credentialPage.Total,
                credentialPage.PageNo);
        }

        public async Task CreateNewUser(NewUser newUser, UserContext userContext = null)
        {
            _logger.LogDebug("Creating new user with login {login}", newUser.Login);
            userContext ??= _contextAccessor.HttpContext?.GetUserContext();
            
            if (await _userCredentialRepository.LoginExists(newUser.Login))
            {
                _logger.LogTrace("Tried to create a new user, but {login} was already taken", newUser.Login);
                throw new Exception("Login already exists");
            }

            int sameNameUserCount = await _userCredentialRepository.CountWithNames(newUser.FirstName, newUser.LastName);
            string generatedUserID = await _userIDGeneration.GenerateUserIDForNewUser(newUser, sameNameUserCount);
            UserRoleExtended role = UserRoleExtended.FromString(newUser.Role) ?? UserRole.DEFAULT;
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
            if (userContext == null)
            {
                //TODO: I question this security myself, should throw an error, but then initial Seed will break
                _logger.LogWarning("User is being created with Login {login}, ID {userId} and Role {role}, but there is no User Context present. ", newUser.Login, generatedUserID, role);
                userContext = new()
                {
                    Login = "Unknown",
                    UserID = "Unknown",
                    Role = UserRole.SYS_ADMIN //since that is basically what it is given
                };
            }
            await _userCredentialRepository.Insert(userCredentials, userContext);
        }

        public async Task<UserCredentials> GetUserCredentialsByLogin(string login)
        {
            _logger.LogDebug("Fetching user credentials for login {login} ", login);
            return await _userCredentialRepository.GetByLogin(login);
        }

        public async Task<UserSystemInfo> GetUserSystemInfoByLogin(string login)
        {
            _logger.LogDebug("Fetching user system info for login {login} ", login);
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
            _logger.LogDebug("Tying to expired user with {login} ", login);
            UserContext userContext = GetUserContextFromHttpOrThrowException();
            UserCredentials credentials = await _userCredentialRepository.GetByLogin(login);
            if (credentials == null)
            {
                _logger.LogTrace("Did not find a user wiht login {login}", login);
                throw new KeyNotFoundException("Login does not exist");
            }

            credentials.IsExpired = true;

            await _userCredentialRepository.Update(credentials, userContext);
        }

        public async Task UpdateUser(UpdatedUser updatedUser)
        {
            _logger.LogDebug("Tying to update user with userId {userId} ", updatedUser.UserID);
            UserContext userContext = GetUserContextFromHttpOrThrowException();
            UserCredentials credentials = await _userCredentialRepository.GetByUserIdWithDetails(updatedUser.UserID);
            if (credentials == null)
            {
                _logger.LogTrace("Did not find a user with userId {login}", updatedUser.UserID);
                throw new KeyNotFoundException("Login does not exist");
            }

            credentials.Role = UserRoleExtended.FromString(updatedUser.Role);
            credentials.UserDetails.FirstName = updatedUser.FirstName;
            credentials.UserDetails.LastName = updatedUser.LastName;
            credentials.UserDetails.Email = updatedUser.Email;
            credentials.UserDetails.Telephone = updatedUser.Telephone;

            await _userCredentialRepository.Update(credentials, userContext);
        }

        public async Task UpdateLastActiveByLogin(string login)
        {
            UserCredentials credentials = await _userCredentialRepository.GetByLogin(login);
            UserContext userContext = _mapper.Map<UserContext>(credentials);
            credentials.LastActive = DateTime.Now;
            await _userCredentialRepository.Update(credentials, userContext);
        }

        public async Task UpdateLastActiveByUserID(string userID)
        {
            UserCredentials credentials = await _userCredentialRepository.GetByUserID(userID);
            UserContext userContext = _mapper.Map<UserContext>(credentials);
            credentials.LastActive = DateTime.Now;
            await _userCredentialRepository.Update(credentials, userContext);
        }

        private UserContext GetUserContextFromHttpOrThrowException()
        {
            return _contextAccessor.HttpContext?.GetUserContext() 
                   ?? throw new Exception("User could not be recognized by the system; Cannot execute");
        }
    }
}
