
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ValuedInBE.AutoMapperProfiles;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls.Memory;
using ValuedInBE.Models;
using ValuedInBE.Repositories;
using ValuedInBE.Repositories.Database;
using ValuedInBE.Services.Users;
using ValuedInBE.Services.Users.Implementations;
using ValuedInBE.System;

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

            builder.Services.AddSingleton(new MapperConfiguration(c => c.AddProfile(new MappingProfile())).CreateMapper());
            builder.Services.AddSingleton<IMemoizationEngine, MemoizationEngine>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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