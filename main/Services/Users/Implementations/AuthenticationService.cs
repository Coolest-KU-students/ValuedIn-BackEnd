
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;
using ValuedInBE.Models.Users;
using ValuedInBE.Security.Users;

namespace ValuedInBE.Services.Users.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;

        private double ExpirationInHours { get; init; }
        private string Issuer { get; init; }
        private string Audience { get; init; }
        private string Key { get; init; }

        public AuthenticationService(ILogger<AuthenticationService> logger, IUserService userService, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userService = userService;
            _hasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
            IConfigurationSection config = configuration.GetRequiredSection("Jwt");
            ExpirationInHours = Convert.ToDouble(config.GetSection("ExpirationInHours").Value);
            Issuer = config.GetSection("Issuer").Value;
            Audience = config.GetSection("Audience").Value;
            Key = config.GetSection("Key").Value;
        }

        public async Task<TokenAndRole> AuthenticateUser(AuthRequest auth)
        {
            UserCredentials credentials = await _userService.GetUserCredentialsByLogin(auth.Login);
            if (credentials == null)
            {
                _logger.LogError("Did not find any credentials with login {login} when trying to authenticate", auth.Login);
                throw new Exception("Incorrect credentials");
            }

            User user = _mapper.Map<User>(credentials);

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

            return new()
            {
                Token = GenerateJWT(user),
                Role = credentials.Role.GetDisplayName()
            };
        }

        public async Task RegisterNewUser(NewUser newUser)
        {
            await HashPasswordAndSave(newUser);
        }

        public async Task SelfRegister(NewUser newUser)
        {
            if (newUser.Role != UserRoleExtended.DEFAULT)
            {
                _logger.LogError("Attempter to Self Register a user with {login}, but role {Role} was not allowed", newUser.Login, newUser.Role);
                throw new Exception("Unallowed User role");
            }
            await HashPasswordAndSave(newUser);
        }

        public async Task<TokenAndRole> VerifyToken(string token)
        {
            _logger.LogTrace("Attempting to verify token {token}", token);

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };
            ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken _);
            string login = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            _logger.LogTrace("Extracted login {login} during verification", login);
            UserCredentials credentials = await _userService.GetUserCredentialsByLogin(login);
            if (credentials.IsExpired) throw new Exception("User is expired");
            credentials.LastActive = DateTime.UtcNow;
            return new()
            {
                Token = GenerateJWT(_mapper.Map<User>(credentials)),
                Role = credentials.Role.GetDisplayName()
            };
        }
        
        private async Task HashPasswordAndSave(NewUser newUser)
        {
            User user = _mapper.Map<User>(newUser);
            newUser.Password = HashPassword(user, newUser.Password);
            _logger.LogTrace("Hashing a password for login {login}", newUser.Login);
            await _userService.CreateNewUser(newUser);
        }

        private string HashPassword(User user, string password)
        {
            string hashPassword = _hasher.HashPassword(user, password);
            return hashPassword;
        }

        private string GenerateJWT(User user)
        {
            SigningCredentials signingCredentials = GetSigningCredentials();
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Login),
            };

            foreach (UserRoleExtended role in UserRoleExtended.GetExtended(user.Role).FlattenRoleHierarchy())
            {
                claims.Add(new Claim(ClaimTypes.Role, (string) role));
            }

            JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Key);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature);
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = 
                new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(ExpirationInHours),
                    signingCredentials: signingCredentials
                );
            return tokenOptions;
        }
    }
}
