using System.Diagnostics;
using ValuedInBE.Models.DTOs.Requests;
using ValuedInBE.Security.Users;
using ValuedInBE.Services.Users;

namespace ValuedInBE.Contexts
{
    public class DataInitializer
    {
        public static async Task Initialize(ValuedInContext context, IAuthenticationService authenticationService) 
        {
            if (context.UserCredentials.Any())
            {
                return;   // DB has been seeded
            }

            await CreateDefaultUser(authenticationService); //Need to create it with a service to hash the password
        }

        private static async Task CreateDefaultUser(IAuthenticationService authenticationService)
        {
            NewUser newUser = new("SetupUser", UserRole.SYS_ADMIN, "Setup", "User", "Password1", "", "");
            await authenticationService.RegisterNewUser(newUser);
        }

    }
}
