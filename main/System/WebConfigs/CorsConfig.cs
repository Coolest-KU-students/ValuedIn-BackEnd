using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ValuedInBE.System.WebConfigs
{
    public class CorsConfig
    {
        public static CorsConfig LocalHostConfig { get; } =
            new()
            {
                CorsConfigName = "LocalHostConfig",
                AllowedOrigins = new string[] { "http://localhost:3000" }
            };

        public string CorsConfigName { get; init; }
        public string[] AllowedOrigins { get; init; }

        internal Action<CorsOptions> Configure() =>
            options => options.AddPolicy(
                    name: CorsConfigName,
                    policy => policy.AllowCredentials()
                                    .WithOrigins(AllowedOrigins)
                                    .AllowAnyHeader()
                                    .AllowCredentials()
                                    .AllowAnyMethod()
                );

    }
}
