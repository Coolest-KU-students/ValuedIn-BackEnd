
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ValuedInBE.Chats.EventHandlers;
using ValuedInBE.Chats.Repositories;
using ValuedInBE.Chats.Services;
using ValuedInBE.DataControls.Memory;
using ValuedInBE.System.Configuration.Environmental;
using ValuedInBE.System.External.Services.Kafka;
using ValuedInBE.System.External.Tools.AutoMapperProfiles;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.System.WebConfigs;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Tokens.Services;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Repositories;
using ValuedInBE.Users.Services;
using ValuedInBE.Users.Services.Implementations;
using ValuedInBE.WebSockets.Services;

namespace ValuedInBE
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            JwtConfiguration jwtConfiguration = new(builder.Configuration);
            CorsConfig corsConfigugration = CorsConfig.LocalHostConfig;
            builder.Services.AddCors(corsConfigugration.Configure());

            builder.Services.AddControllers();

            string connectionString = EnvironmentalConnectionStringBuilder.BuildConnectionString();
            builder.Services.AddDbContext<ValuedInContext>(options => options.UseSqlServer(connectionString));

            #region Scoped
            builder.Services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPasswordHasher<UserData>, PasswordHasher<UserData>>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserIDGenerationStrategy, CustomUserIDGenerationStrategyWithNameMerging>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IUserContextAccessor, UserContextAccessor>();
            #endregion

            #region Singletons  
            builder.Services.AddSingleton(jwtConfiguration);
            builder.Services.AddSingleton(typeof(IKafkaConfigurationBuilder<,>), typeof(KafkaConfigurationBuilder<,>));
            builder.Services.AddSingleton<ActiveWebSocketTracker>();
            builder.Services.AddSingleton<MessageEventHandler>();
            builder.Services.AddSingleton(new MapperConfiguration(c => c.AddProfile(new MappingProfile())).CreateMapper());
            builder.Services.AddSingleton<IMemoizationEngine, MemoizationEngine>();
            builder.Services.AddSingleton<IWebSocketTracker>(x => x.GetRequiredService<ActiveWebSocketTracker>());
            builder.Services.AddSingleton<IMessageEventHandler>(x => x.GetRequiredService<MessageEventHandler>());
            builder.Services.AddSingleton<ITokenService, TokenService>();
            #endregion

            #region Hosted Services
            builder.Services.AddHostedService(x => x.GetRequiredService<ActiveWebSocketTracker>());
            builder.Services.AddHostedService(x => x.GetRequiredService<MessageEventHandler>());
            #endregion

            builder.Services.AddTransient<UserContextMiddleware>();
            builder.Services.AddTransient<ExceptionTranslationMiddleware>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            #region JWT Configurer

#if DEBUG   //Testing will add its own Authentication layer
            if (!TestRecognizer.IsTestingEnvironment)
            {
#endif
                builder.Services.ConfigureJwt(jwtConfiguration);
                builder.Services.AddAuthorization();
#if DEBUG
            }
#endif
            #endregion
            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (IServiceScope scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                ValuedInContext context = services.GetRequiredService<ValuedInContext>();
                IAuthenticationService authenticationService = services.GetRequiredService<IAuthenticationService>();
                IUserContextAccessor userContextAccessor = services.GetRequiredService<IUserContextAccessor>();

                context.Database.EnsureCreated();
                await DataInitializer.InitializeAsync(context, authenticationService, userContextAccessor);
            }

            app.UseHttpsRedirection();
            app.UseWebSockets();
            app.UseMiddleware<ExceptionTranslationMiddleware>();
            app.UseMiddleware<UserContextMiddleware>();
            app.UseCors(corsConfigugration.CorsConfigName);
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.MapControllers();
            app.Run();
        }
    }
}