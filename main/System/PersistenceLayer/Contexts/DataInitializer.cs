using ValuedInBE.System.Security.Users;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Services;

namespace ValuedInBE.System.PersistenceLayer.Contexts
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
            NewUser newUser = new()
            {
                Login = "SetupUser",
                Role = UserRoleExtended.SYS_ADMIN,
                FirstName = "Setup",
                LastName = "User",
                Password = "Password1",
                Telephone = "",
                Email = ""
            };
            await authenticationService.RegisterNewUser(newUser);
        }

    }
}
