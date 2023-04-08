using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ValuedInBE.System.WebConfigs
{
    public class JwtConfiguration
    {
        private const string audienceSection = "Audience";
        private const string issuerSection = "Issuer";
        private const string keySection = "Key";
        private const string expirationInHoursSection = "ExpirationInHours";
        private const string jwtSectionName = "JWT";
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly IConfigurationSection _jwtConfigurationSection;
        public DateTime ExpirationDateTime => GetExpirationDateTime();
        public string Audience => GetJWTValue(audienceSection);
        public string Issuer => GetJWTValue(issuerSection);
        public TokenValidationParameters TokenValidationParameters => GetTokenValidationParameters();
        private string Key => GetJWTValue(keySection);
        private SymmetricSecurityKey SecurityKey => new(_encoding.GetBytes(Key));
        private SigningCredentials SigningCredentials => new(SecurityKey, SecurityAlgorithms.HmacSha512Signature);

        public JwtConfiguration(IConfiguration configuration)
        {
            _jwtConfigurationSection = configuration.GetRequiredSection(jwtSectionName);
        }

        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            return new JwtSecurityTokenHandler().ValidateToken(token, TokenValidationParameters, out SecurityToken _);
        }

        public JwtSecurityToken GenerateSecurityToken(IEnumerable<Claim> claims)
        {
            return new(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: ExpirationDateTime,
                    signingCredentials: SigningCredentials
                );
        }

        public string GenerateJWT(IEnumerable<Claim> claims)
        {
            return new JwtSecurityTokenHandler().WriteToken(GenerateSecurityToken(claims));
        }

        private DateTime GetExpirationDateTime()
        {
            double expirationInHours = Convert.ToDouble(GetJWTValue(expirationInHoursSection));
            return DateTime.Now.AddHours(expirationInHours);
        }

        private string GetJWTValue(string sectionName)
        {
            return _jwtConfigurationSection.GetSection(sectionName).Value;
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new()
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = SecurityKey,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }
    }

    public static class JwtServiceExtension
    {
        public static void ConfigureJwt(this IServiceCollection services, JwtConfiguration jwtConfiguration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => options.TokenValidationParameters = jwtConfiguration.TokenValidationParameters);
        }
    }
}
