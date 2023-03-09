using Microsoft.EntityFrameworkCore;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls.Ordering;
using ValuedInBE.DataControls.Ordering.Internal;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.Users;
using ValuedInBE.System.Extensions;

namespace ValuedInBE.Repositories.Database
{

    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly CustomColumnMapping<UserCredentials> _customColumnMapping = new()
        {
            {"LastName",  new OrderingExpression<UserCredentials>(creds => creds.UserDetails.LastName + creds.UserDetails.FirstName) }
        };

        private readonly ValuedInContext _context;
        private readonly ILogger<UserCredentialRepository> _logger;

        public UserCredentialRepository(ValuedInContext context, ILogger<UserCredentialRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CountWithNames(string firstName, string lastName)
        {
            _logger.LogTrace("Requested user with First Name '{firstName}', Last Name '{lastName}'", firstName, lastName);
            return await _context.UserDetails.CountAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        public async Task<List<UserCredentials>> GetAllUsers()
        {
            _logger.LogTrace("All User List Requested");
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails)
                                  select c;
            return await credentialQuery.ToListAsync();
        }

        public async Task<UserCredentials> GetByLogin(string login)
        {
            _logger.LogTrace("Request user credentials with login {login}", login);
            var credentialQuery = from c in _context.UserCredentials
                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }
        public async Task<UserCredentials> GetByUserID(string userID)
        {
            _logger.LogTrace("Request user credentials with userID {userID}", userID);
            var credentialQuery = from c in _context.UserCredentials
                                  where c.UserID == userID
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }

        public async Task<UserCredentials> GetByLoginWithDetails(string login)
        {
            _logger.LogTrace("Request user credentials and details with login {login}", login);
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)

                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }

        public async Task<UserCredentials> GetByUserIdWithDetails(string userID)
        {
            _logger.LogTrace("Request user credentials and details with user ID {userId}", userID);
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)

                                  where c.UserID == userID
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }


        public async Task<Page<UserCredentials>> GetUserPageWithDetails(UserPageRequest config)
        {
            _logger.LogTrace("Request Requested {size} User Page {page}", config.Size, config.Page);

            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails)
                                            .ApplyOrderingInLinq(config.OrderByColumns, _customColumnMapping)
                                            .Where(a => a.IsExpired == config.ShowExpired)
                                  select c;

            int total = credentialQuery.Count();
            credentialQuery.Skip(config.Page * config.Size);
            credentialQuery.Take(config.Size);

            List<UserCredentials> credentials = await credentialQuery.ToListAsync();

            return new Page<UserCredentials>(credentials, total, config.Page + 1);
        }

        public async Task Insert(UserCredentials userCredentials, UserContext createdBy)
        {
            _logger.LogTrace("Inserting user with login {login}", userCredentials.Login);
            _context.UserCredentials.Add(userCredentials);
            _context.ChangeTracker.CheckAuditing(createdBy);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LoginExists(string login)
        {
            _logger.LogTrace("Inserting user with login {login}", login);
            return await _context.UserCredentials.AnyAsync(c => c.Login == login);
        }

        public async Task Update(UserCredentials userCredentials, UserContext updatedBy)
        {
            _logger.LogTrace("Updating user with login {login}", userCredentials.Login);
            _context.UserCredentials.Update(userCredentials);
            _context.ChangeTracker.CheckAuditing(updatedBy);
            await _context.SaveChangesAsync();
        }
    }
}
