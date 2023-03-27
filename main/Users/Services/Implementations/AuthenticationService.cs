
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValuedInBE.System.Security.Users;
using ValuedInBE.System.WebConfigs;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<UserData> _hasher;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        private readonly JwtConfiguration _jwtConfigurer;
        private readonly IHttpContextAccessor _contextAccessor;


        public AuthenticationService(ILogger<AuthenticationService> logger, IUserService userService,  IPasswordHasher<UserData> passwordHasher, IMapper mapper, JwtConfiguration jwtConfigurer)
        {
            _userService = userService;
            _hasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
            _jwtConfigurer = jwtConfigurer;
        }

        public async Task<TokenAndRole> AuthenticateUserAsync(AuthRequest auth)
        {
            _logger.LogTrace("Authenticating user with login {login}", auth.Login);
            UserCredentials credentials = await _userService.GetUserCredentialsByLoginAsync(auth.Login);
            if (credentials == null)
            {
                _logger.LogError("Did not find any credentials with login {login} when trying to authenticate", auth.Login);
                throw new Exception("Incorrect credentials");
            }

            UserData user = _mapper.Map<UserData>(credentials);

            PasswordVerificationResult passwordVerification = _hasher.VerifyHashedPassword(user, credentials.Password, auth.Password);
            switch (passwordVerification)
            {
                case PasswordVerificationResult.Failed:
                    _logger.LogError("Incorrect password provided for {login}", auth.Login);
                    throw new Exception("Incorrect credentials");
                case PasswordVerificationResult.SuccessRehashNeeded:
                    //TODO: implement
                    _logger.LogError("Password for user {auth.Login} needs to be rehashed", auth.Login);
                    break;
            }

            Task updateLastActiveTask = _userService.UpdateLastActiveAsync(credentials);

            TokenAndRole tokenAndRole =  new()
            {
                Token = GenerateJWT(user),
                Role = credentials.Role.GetDisplayName()
            };

            await updateLastActiveTask;
            return tokenAndRole;
        }

        public async Task RegisterNewUserAsync(NewUser newUser)
        {
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();

            if(userContext != null && userContext.Role == UserRole.SYS_ADMIN)
            {
                _logger.LogDebug("Request to register {newUser.Login} is authenticated", newUser.Login);
                await HashPasswordAndSave(newUser);
                return;
            }

            _logger.LogDebug("Request to register {newUser.Login} is from {role}", newUser.Login, userContext?.Role.GetDisplayName() ?? "Unauthenticated");
            await SelfRegisterAsync(newUser);
        }

        public async Task SelfRegisterAsync(NewUser newUser)
        {
            if (newUser.Role != UserRoleExtended.DEFAULT)
            {
                _logger.LogError("Attempter to Self Register a user with {login}, but role {Role} was not allowed", newUser.Login, newUser.Role);
                throw new Exception("Unallowed User role");
            }
            UserContext userContext = new()
            {
                UserID = null,
                Login = newUser.Login,
                Role = UserRole.DEFAULT
            };
            await HashPasswordAndSave(newUser, userContext);
        }

        public async Task<UserCredentials> GetUserFromTokenAsync(string token)
        {
            _logger.LogTrace("Attempting to fetch UserCredentials from token {token}", token);
            ClaimsPrincipal claimsPrincipal = _jwtConfigurer.GetClaimsFromToken(token); 
            string login = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            _logger.LogDebug("Extracted login {login} from token ", login);
            return await _userService.GetUserCredentialsByLoginAsync(login);
        }

        public async Task<TokenAndRole> VerifyAndIssueNewTokenAsync(string token)
        {
            UserCredentials credentials = await GetUserFromTokenAsync(token);
            if (credentials.IsExpired) 
                throw new Exception("User is expired");

            Task updatLastActiveTask = _userService.UpdateLastActiveAsync(credentials);
            TokenAndRole tokenAndRole = new()
            {
                Token = GenerateJWT(_mapper.Map<UserData>(credentials)),
                Role = credentials.Role.GetDisplayName()
            };
            await updatLastActiveTask;
            return tokenAndRole;
        }

        private async Task HashPasswordAndSave(NewUser newUser, UserContext creatorContext = null)
        {
            UserData user = _mapper.Map<UserData>(newUser);
            newUser.Password = HashPassword(user, newUser.Password);
            _logger.LogTrace("Hashing a password for login {login}", newUser.Login);
            await _userService.CreateNewUserAsync(newUser, creatorContext);
        }

        private string HashPassword(UserData user, string password)
        {
            string hashPassword = _hasher.HashPassword(user, password);
            return hashPassword;
        }

        private string GenerateJWT(UserData user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.UserID)
            };

            foreach (UserRoleExtended role in UserRoleExtended.GetExtended(user.Role).FlattenRoleHierarchy())
            {
                claims.Add(new Claim(ClaimTypes.Role, (string)role));
            }

            string token = _jwtConfigurer.GenerateJWT(claims);
            return token;
        }
    }
}
