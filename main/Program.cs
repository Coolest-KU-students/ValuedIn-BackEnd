
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PostSharp.Extensibility;
using System.Text;
using System.Text.Json.Serialization;
using ValuedInBE.AutoMapperProfiles;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls.Memory;
using ValuedInBE.Events.Handlers;
using ValuedInBE.Models;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.Models.Events;
using ValuedInBE.Repositories;
using ValuedInBE.Repositories.Database;
using ValuedInBE.Services.Chats;
using ValuedInBE.Services.Chats.Implementations;
using ValuedInBE.Services.Tokens;
using ValuedInBE.Services.Users;
using ValuedInBE.Services.Users.Implementations;
using ValuedInBE.System;
using ValuedInBE.System.Kafka;
using ValuedInBE.System.Middleware;
using ValuedInBE.System.WebConfigs;

namespace ValuedInBE
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            CorsConfig corsConfig = CorsConfig.LocalHostConfig;

            builder.Services.AddCors(corsConfig.Configure());

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ValuedInContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ValuedIn")));

            //Adding Repositories
            builder.Services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserIDGenerationStrategy, CustomUserIDGenerationStrategyWithNameMerging>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
           

            builder.Services.AddSingleton(new MapperConfiguration(c => c.AddProfile(new MappingProfile())).CreateMapper());
            builder.Services.AddSingleton<IMemoizationEngine, MemoizationEngine>();     
            builder.Services.AddSingleton<IKafkaConfigurationBuilder<long, NewMessageEvent>, KafkaConfigurationBuilder<long, NewMessageEvent>>();
            builder.Services.AddSingleton<ActiveWebSocketTracker>();
            builder.Services.AddSingleton<MessageEventHandler>();
            builder.Services.AddSingleton<IWebSocketTracker>(x => x.GetRequiredService<ActiveWebSocketTracker>());
            builder.Services.AddSingleton<IMessageEventHandler>(x => x.GetRequiredService<MessageEventHandler>());
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<ITokenService, TokenService>();

            builder.Services.AddHostedService(x => x.GetRequiredService<ActiveWebSocketTracker>());
            builder.Services.AddHostedService(x => x.GetRequiredService<MessageEventHandler>());

            builder.Services.AddTransient<UserContextMiddleware>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
#if DEBUG
            if (!TestRecognizer.IsTestingEnvironment)
            {  //Testing will add its own Authentication layer
#endif
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false, //TODO: investigate
                        ValidateIssuerSigningKey = false
                    };
                });

                builder.Services.AddAuthorization();
#if DEBUG
            }
#endif
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

                context.Database.EnsureCreated();
                await DataInitializer.Initialize(context, authenticationService);
            }

            app.UseHttpsRedirection();
            app.UseWebSockets();
            app.UseMiddleware<UserContextMiddleware>();
            app.UseCors(corsConfig.CorsConfigName);
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.MapControllers();
            app.Run();
        }
    }
}