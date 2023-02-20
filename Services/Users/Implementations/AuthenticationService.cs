
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.Users;
using ValuedInBE.Security.Users;

namespace ValuedInBE.Services.Users.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IConfigurationSection _configuration;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;

        public AuthenticationService(ILogger<AuthenticationService> logger, IUserService userService, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userService = userService;
            _configuration = configuration.GetRequiredSection("Jwt");
            _hasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> AuthenticateUser(AuthRequest auth)
        {
            UserCredentials credentials = await _userService.GetUserCredentialsByLogin(auth.Login);
            if (credentials == null)
            {
                throw new Exception("Incorrect credentials");
            }

            User user = _mapper.Map<User>(credentials);

            PasswordVerificationResult passwordVerification = _hasher.VerifyHashedPassword(user, credentials.Password, auth.Password);
            switch (passwordVerification)
            {
                case PasswordVerificationResult.Failed:
                    throw new Exception("Incorrect credentials");
                case PasswordVerificationResult.SuccessRehashNeeded:
                    //TODO: implement
                    _logger.LogError("Password for user {auth.Login} needs to be rehashed", auth.Login);
                    break;
            }

            return GenerateJWT(user);
        }

        public async Task RegisterNewUser(NewUser newUser)
        {
            await HashPasswordAndSave(newUser);
        }

        public async Task SelfRegister(NewUser newUser)
        {
            if (newUser.Role != UserRole.DEFAULT)
            {
                throw new Exception("Unallowed User role");
            }
            await HashPasswordAndSave(newUser);
        }

        private async Task HashPasswordAndSave(NewUser newUser)
        {
            User user = _mapper.Map<User>(newUser);
            newUser.Password = HashPassword(user, newUser.Password);
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
                claims.Add(new Claim(ClaimTypes.Role, (string)role));
            }

            JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Key")?.Value ?? throw new Exception("NO KEY FOUND"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature);
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.GetSection("Issuer").Value,
                audience: _configuration.GetSection("Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(_configuration.GetSection("ExpirationInHours").Value)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

    }
}
