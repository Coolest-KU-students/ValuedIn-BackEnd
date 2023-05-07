using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.Security.Users;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Services;
using ValuedInBEIntegrationTests.Data;
using ValuedInBETests.IntegrationTests.Data;

namespace ValuedInBETests.IntegrationTests.Config
{
    public class IntegrationTestWebApplicationFactory<TClass>
        : WebApplicationFactory<TClass>
            where TClass : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(async services =>
            {
                var dbContextDescriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ValuedInContext>));

                services.Remove(dbContextDescriptor!);

                var dbConnectionDescriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

                services.Remove(dbConnectionDescriptor!);

                var connection = new SqliteConnection("DataSource=myshareddb;mode=memory;");
                connection.Open();

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    return connection;
                });

                services.AddDbContext<ValuedInContext>((container, options) =>
                {
                    DbConnection connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                ServiceProvider provider = services.BuildServiceProvider();
                using (IServiceScope scope = provider.CreateScope())
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    ValuedInContext context = scopedServices.GetRequiredService<ValuedInContext>();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    IAuthenticationService authenticationService = scopedServices.GetRequiredService<IAuthenticationService>();
                    IUserContextAccessor userContextAccessor = scopedServices.GetRequiredService<IUserContextAccessor>();
                    await UserTestDataInitializer.Initialize(authenticationService, userContextAccessor);
                    await PersonalValueTestDataInitializer.Initialize(context);
                    context.SaveChanges();
                }
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultLogin = UserRoleExtended.SYS_ADMIN);

                services.AddAuthentication(TestAuthHandler.authenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.authenticationScheme, options => { });

                services.AddAuthorization();
            });

            builder.UseEnvironment("Development");
        }
    }
}
